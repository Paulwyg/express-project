using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Threading;

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.DataValidation;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj2090Module.Wj2090InfoSet.Services;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj2090Module.Services;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.ControlInfoSet.ViewModel;
 
using System.Windows;
using DragDropExtend.ExtensionsHelper;
using System.Windows.Controls;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.client;

//using Wlst.Ux.Wj2090Module.Wj2090InfoSet.Services;

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet.ViewModel
{
    [Export(typeof (IIConcentratorParaInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ConcentratorParaInformationViewModel :Wlst .Cr .Core .EventHandlerHelper .EventHandlerHelperExtendNotifyProperyChanged , IIConcentratorParaInformationViewModel
    {

        public ConcentratorParaInformationViewModel()
        {           
            InitAciton();
            InitEvent();
        }

        public void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId, PublishEventType.Core,true );
            this.AddEventFilterInfo(Wj2090Module.Services.EventIdAssign.GrpConInfoUpdateId , PublishEventType.Core,true );
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
       

            if (_terminalInformation == null) return;

            try
            {
                //tmlinfo update
                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                {
                    var info = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (info == null) return;
                    if (info.Any(g => g.Item1 == SingleId))
                    {
                        this.NavOnLoad(SingleId);

                        if (DateTime.Now.Ticks - dtsnd < 600000000)
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 集中器参数保存成功!!!";
                        }
                        else
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收到服务器更新终端参数信息，执行本页面的数据更新!!!";
                        }
                    }
                } 
                if (args.EventId == Wj2090Module.Services.EventIdAssign.GrpConInfoUpdateId )
                {
                    var info= Convert .ToInt32( args.GetParams()[0]) ;
                    if (info <1) return;
                    if (info != _terminalInformation.RtuId) return;
                    
                        this.InitGroupViewModel();

                        if (DateTime.Now.Ticks - dtsnd < 600000000)
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 集中器分组参数保存成功!!!";
                        }
                        else
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收到服务器更新终端参数信息，执行本页面的数据更新!!!";
                        }
                    
                }

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        #region Msg

        private string _btMsg;

        public string Msg
        {
            get { return _btMsg; }
            set
            {
                if (_btMsg == value) return;
                _btMsg = value;
                RaisePropertyChanged(() => Msg);
            }
        }

        #endregion

        #region IITab
        public int Index
        {
            get { return 1; }
        }
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
            get { return "单灯参数设置"; }
        }

        #endregion

        private bool isViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            IsFidVisi = Visibility.Collapsed;
            ParaType = 1;
            isViewActive = true;
            grpFlag = true;
            ShowSndInfo = "";
            int singleId = Convert.ToInt32(parsObjects[0]);
            if (singleId > 0)
            {
                SelectedSingleChange(singleId);
            }
            IsEnableCore = false;
            existBarCode = false;
            ScanMode = false;
        }

        public void OnUserHideOrClosing()
        {
            IsFidVisi = Visibility.Collapsed;
            isViewActive = false;
            ShowSndInfo = "";
            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }
            ControlParaItems.Clear();
            TreeItems.Clear();
        }
    }

    /// <summary>
    /// 集中器基本属性
    /// </summary>
    public partial class ConcentratorParaInformationViewModel
    {

        private void InitConcentratorViewModel()
        {
            if (_terminalInformation == null)
                return;

            CommunicationType = _terminalInformation.RtuFid != 0;
            this.SingleName = _terminalInformation.RtuName;
            SingleId = _terminalInformation.RtuId;
            FId = _terminalInformation.RtuFid ;
            PhyId = _terminalInformation.RtuPhyId ;
            IsAllowPatrol = _terminalInformation.WjSlu . IsPartrolMeasured;
            IsAllowActiveAlarm = _terminalInformation.WjSlu.IsAlarmAuto;
            IsReissue = _terminalInformation.WjSlu.IsSndOrderAuto;
            IsMeasure = false;
            ARange = _terminalInformation.WjSlu.CurrentUpper;
            PRange = _terminalInformation.WjSlu.PowerUpper;
            ControlNum = _terminalInformation.WjSlu.SumOfControls;
            IsZigbee = _terminalInformation.WjSlu.IsZigbee == 1;

            DomainName = _terminalInformation.WjSlu.DomainName;
            DomainNamePower = _terminalInformation.WjSlu.DomainName;

            RelatedRtuId = _terminalInformation.WjSlu.RelatedRtuId;

            int tmp = 0;
            if (int.TryParse(_terminalInformation.RtuArgu, out tmp))IsAllowPatrolOnLight = tmp ==1; 

            foreach (var tttt in IsCommunicationChannel)
            {
                if (_terminalInformation.WjSlu.ChannelUsed.Contains(tttt.Value))
                    tttt.IsSelected = true;
                else tttt.IsSelected = false;
            }

            PCode = _terminalInformation.WjSlu.BluetoothPin;
            SafeMode = _terminalInformation.WjSlu.SecurityPattern;
            RouteRunMode = _terminalInformation.WjSlu.RouteRunPattern;
            CommunicationFailureNum = _terminalInformation.WjSlu.AlarmCountCommucationFail;
            VAlarmMax = _terminalInformation.WjSlu.UpperVoltage;
            PowerFactor = _terminalInformation.WjSlu.AlarmPowerfactorLower;
            VAlarmMin = _terminalInformation.WjSlu.LowerVoltage;

           

            if (_terminalInformation.WjSlu.PowerAdjustType == 1)
            {
                PowerControl = false;
                Frequency = _terminalInformation.WjSlu.PowerAdjustBound;
            }
            else
            {
                PowerControl = true;
                foreach (var g in BaudItems)
                {
                    if (g.Value == _terminalInformation.WjSlu.PowerAdjustBound)
                    {
                        CurrentBaudItem = g;
                        break;
                    }
                }
                // CurrentBaudItem.Value = _terminalInformation.PowerAdjustBound;
            }
            PhoneNum = _terminalInformation.WjSlu.MobileNo;
           // IPAdress = _terminalInformation.;
            InstallTime = new DateTime(_terminalInformation.DateCreate );
            Remark = _terminalInformation.RtuRemark ;

            Latitude = _terminalInformation.WjSlu.Latitude;
            Longitude = _terminalInformation.WjSlu.Longitude;
            IsStopRun = _terminalInformation.WjSlu.IsUsed == false;

            //IsAllowPatrolOnLight=_terminalInformation.WjSlu.  //todo
            
            //tmp.SluRegulators.DicRtuParaSluRegulator
        }

        private Wlst .client .EquipmentParameter BackConcentratorViewModelEqu()
        {
            return new EquipmentParameter()
                       {
                           //AreaId = _terminalInformation.AreaId,
                           DateCreate = _terminalInformation.DateCreate,
                           DateUpdate = DateTime.Now.Ticks,
                           RtuId = this.SingleId,
                           RtuName = this.SingleName,
                           RtuPhyId = PhyId,
                           RtuFid = FId,
                           RtuGisX = _terminalInformation.RtuGisX,
                           RtuGisY = _terminalInformation.RtuGisY,
                           RtuArgu =this.IsAllowPatrolOnLight? "1":"0",// _terminalInformation.RtuArgu,
                           RtuInstallAddr = _terminalInformation.RtuInstallAddr,
                           
                           RtuMapX = _terminalInformation.RtuMapX,
                           RtuMapY = _terminalInformation.RtuMapY,
                           RtuModel = _terminalInformation.RtuModel,
                           RtuStateCode = 2,
                           RtuRemark = Remark,

                           

                       };
        }


        private SluParameter  BackConcentratorViewModelSlu()
        {
            return new SluParameter()
                       {
                           AlarmCountCommucationFail = CommunicationFailureNum,
                           IsPartrolMeasured = this.IsAllowPatrol,
                           IsSndOrderAuto = this.IsReissue,
                           CurrentUpper = this.ARange,
                           PowerUpper = this.PRange,
                           SumOfControls = ControlNum,
                           DomainName =IsZigbee? DomainName:DomainNamePower,  //lvf 2018年4月2日16:16:10 添加电力载波  域名
                           IsAlarmAuto = this.IsAllowActiveAlarm,
                           IsZigbee = this.IsZigbee ? 1 : 0,

                           BluetoothPin = this.PCode,
                           SecurityPattern = this.SafeMode,
                           RouteRunPattern = this.RouteRunMode,
                           UpperVoltage = this.VAlarmMax,
                           AlarmPowerfactorLower = this.PowerFactor,
                           LowerVoltage = this.VAlarmMin,
                           MobileNo = this.PhoneNum,
                           Longitude = this.Longitude,
                           Latitude = this.Latitude,
                           IsUsed = IsStopRun == false,
                           ChannelUsed = (from t in IsCommunicationChannel where t.IsSelected select t.Value).ToList(),
                           PowerAdjustType = PowerControl ? 2 : 1,
                           PowerAdjustBound =
                               PowerControl ? CurrentBaudItem == null ? 2400 : CurrentBaudItem.Value : Frequency,
                           ZigbeeAddress = this.ZgbAddress,
                           RtuId = _terminalInformation.RtuId,
                           StaticIp = 0,

                           RelatedRtuId = this.RelatedRtuId,

                       };

 
        }

        private bool CanSaveAll()
        {
            if (FId == 0 && CommunicationType)
            {
                //WlstMessageBox.Show("设置错误，必须勾选  GPRS通信",
                //                           "父设备地址设置为0但设置为485通信设备...", WlstMessageBoxType.Ok);
                //return false;
                CommunicationType = false;
            }

            if (FId > 0 && CommunicationType == false)
            {
                //WlstMessageBox.Show("设置错误，父设备地址必须设置为0",
                //                           "设置为485通信设备但父设备地址不为0...", WlstMessageBoxType.Ok);
                //return false;
                CommunicationType = true;
            }
            if (_terminalInformation.RtuFid  != FId)
            {
                if (_terminalInformation.RtuFid  == 0 && FId > 0)
                {
                    var ntg = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( FId);
                    if (ntg == null)
                    {
                        WlstMessageBox.Show("主设备设置错误,无法继续保存",
                                            "系统不存在逻辑地址为 " + FId + " 的设备...", WlstMessageBoxType.Ok);
                        return false;
                    }
                    if (ntg.RtuFid  > 0)
                    {
                        WlstMessageBox.Show("主设备属性错误，无法继续保存",
                                            "逻辑地址为 " + FId + " 的设备不是主设备...", WlstMessageBoxType.Ok);
                        return false;
                    }
                    //Gprs通信转换为485设备通信，修改后
                }
                if (_terminalInformation.RtuFid  > 0 && FId == 0)
                {
                    foreach (var g in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
                        )
                    {
                        if (g.Value.RtuFid  == 0 && g.Key != _terminalInformation.RtuId && g.Value .EquipmentType ==WjParaBase.EquType.Slu )
                        {
                            if (g.Value.RtuPhyId  == PhyId)
                            {
                                WlstMessageBox.Show("物理地址重复",
                                                    "逻辑地址为 " + g.Key + " 的设备已经使用物理地址" + PhyId + "...",
                                                    WlstMessageBoxType.Ok);
                                return false;
                            }
                        }
                    }
                    //Gprs通信转换为485设备通信，修改后
                }
            }
            return true;
        }

        /// <summary>
        /// 显示参数类型
        /// </summary>

        #region ParaType
        private int _paraType;

        public int ParaType
        {
            get { return _paraType; }
            set
            {
                if (_paraType == value) return;
                _paraType = value;
                RaisePropertyChanged(() => ParaType);

                if(value ==1)
                {
                    SndCtrlParaVisi = Visibility.Collapsed;
                    SndSluParaVisi = Visibility.Visible;
                    return;
                }
                if (value == 2)
                {
                    SndCtrlParaVisi = Visibility.Visible ;
                    SndSluParaVisi = Visibility.Collapsed;
                    return;
                }
                if (value == 3)
                {
                    SndCtrlParaVisi = Visibility.Collapsed;
                    SndSluParaVisi = Visibility.Visible;
                    return;
                }
                if (value == 4)
                {
                    SndCtrlParaVisi = Visibility.Collapsed;
                    SndSluParaVisi = Visibility.Collapsed;
                    return;
                }
            }
        }

        #endregion

        /// <summary>
        /// 集中器名称
        /// </summary>

        #region SingleName
        private string _singleName;


        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string SingleName
        {
            get { return _singleName; }
            set
            {
                if (_singleName == value) return;
                _singleName = value;
                RaisePropertyChanged(() => SingleName);
            }
        }

        #endregion

        /// <summary>
        /// 通讯方式 false为GPRS；true为485
        /// </summary>

        #region CommunicationType
        private bool _communicationType;

        public bool CommunicationType
        {
            get { return _communicationType; }
            set
            {
                if (_communicationType == value) return;
                _communicationType = value;
                if (_communicationType)
                    this.SingleId = 1;
                //else
                //    this.SingleId = 0;
                RaisePropertyChanged(() => CommunicationType);
                RaisePropertyChanged(() => IsGprs);
            }
        }

        public bool IsGprs
        {
            get { return !_communicationType; }

        }

        #endregion

        /// <summary>
        /// 集中器物理地址
        /// </summary>

        #region SingleId


        private int _phyId;


        [Range(1,10000.99,ErrorMessage ="物理地址大小介于1至10000之间")]
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId == value) return;
                _phyId = value;
                RaisePropertyChanged(() => PhyId);
            }
        }


        private int _relatedRtuId;


        [Range(0, 99999, ErrorMessage = "终端物理地址大小介于0至99999之间")]
        public int RelatedRtuId
        {
            get { return _relatedRtuId; }
            set
            {
                if (_relatedRtuId == value) return;
                _relatedRtuId = value;
                RaisePropertyChanged(() => RelatedRtuId);
            }
        }

        private int _singleId;

        public int SingleId
        {
            get { return _singleId; }
            set
            {
                if (_singleId == value) return;
                _singleId = value;
                RaisePropertyChanged(() => SingleId);

                var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                ShowSluId = tmps == null ? value+"" : tmps.RtuPhyId.ToString("d4");
 
            }
        }

        private string _singShowSluIdleId;

        public string ShowSluId
        {
            get { return _singShowSluIdleId; }
            set
            {
                if (_singShowSluIdleId == value) return;
                _singShowSluIdleId = value;
                RaisePropertyChanged(() => ShowSluId);
            }
        }

        private int _singlexfId;

        public int FId
        {
            get { return _singlexfId; }
            set
            {
                if (_singlexfId == value) return;
                _singlexfId = value;
                RaisePropertyChanged(() => FId);
                IsFidVisi = Visibility.Collapsed;
                //if (value > 0) IsFidVisi = Visibility.Visible;
                //else
                //{
                //    if (IsEnableCore) IsFidVisi = Visibility.Visible;
                //    else IsFidVisi = Visibility.Collapsed;
                //}
            }
        }

        private bool _sinsdfsdffId;

        public bool IsEnableCore
        {
            get { return _sinsdfsdffId; }
            set
            {
                if (_sinsdfsdffId == value) return;
                _sinsdfsdffId = value;
                RaisePropertyChanged(() => IsEnableCore);
                //if (value)
                //{
                //    IsFidVisi = Visibility.Visible;
                //}
                //else
                //{

                //    if (FId > 0) IsFidVisi = Visibility.Visible;
                //    else IsFidVisi = Visibility.Collapsed;
                //}
                IsFidVisi = Visibility.Collapsed;
            }
        }


        private Visibility _sinsIsFidVisidfsdffId;

        public Visibility IsFidVisi
        {
            get { return _sinsIsFidVisidfsdffId; }
            set
            {
                if (_sinsIsFidVisidfsdffId == value) return;
                _sinsIsFidVisidfsdffId = value;
                RaisePropertyChanged(() => IsFidVisi);
            }
        }

        #endregion

        /// <summary>
        /// 停运
        /// </summary>

        #region IsStopRun
        private bool _isStopRun;

        public bool IsStopRun
        {
            get { return _isStopRun; }
            set
            {
                if (_isStopRun == value) return;
                _isStopRun = value;
                RaisePropertyChanged(() => IsStopRun);
            }
        }

        #endregion

        /// <summary>
        /// 允许巡测
        /// </summary>

        #region IsAllowPatrol
        private bool _isAllowPatrol;

        public bool IsAllowPatrol
        {
            get { return _isAllowPatrol; }
            set
            {
                if (_isAllowPatrol == value) return;
                _isAllowPatrol = value;
                RaisePropertyChanged(() => IsAllowPatrol);
            }
        }



        #endregion



        /// <summary>
        /// 允许开关灯巡测
        /// </summary>
        #region IsAllowPatrolOnLight
        private bool _isAllowPatrolOnLight;

        public bool IsAllowPatrolOnLight
        {
            get { return _isAllowPatrolOnLight; }
            set
            {
                if (_isAllowPatrolOnLight == value) return;
                _isAllowPatrolOnLight = value;
                RaisePropertyChanged(() => IsAllowPatrolOnLight);
            }
        }



        #endregion

        /// <summary>
        /// IsZigbee
        /// </summary>

        #region IsZigbee
        private bool _iIsZigbee;

        public bool IsZigbee
        {
            get { return _iIsZigbee; }
            set
            {
                if (_iIsZigbee == value) return;
                _iIsZigbee = value;

                if (_iIsZigbee)
                {
                    DomaNameVisi = Visibility.Collapsed;
                }
                else
                {
                    DomaNameVisi = Visibility.Visible;
                }
                
                RaisePropertyChanged(() => IsZigbee);
            }
        }



        #endregion


        #region  电力载波 域名 可见
        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility DomaNameVisi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.DomaNameVisi);
                }
            }
        }
        #endregion





        //#endregion

        /// <summary>
        /// 允许主动报警
        /// </summary>

        #region IsAllowActiveAlarm
        private bool _isAllowActiveAlarm;

        public bool IsAllowActiveAlarm
        {
            get { return _isAllowActiveAlarm; }
            set
            {
                if (_isAllowActiveAlarm == value) return;
                _isAllowActiveAlarm = value;
                RaisePropertyChanged(() => IsAllowActiveAlarm);
            }
        }

        #endregion

        /// <summary>
        /// 自动补发指令
        /// </summary>

        #region IsReissue
        private bool _isReissue;

        public bool IsReissue
        {
            get { return _isReissue; }
            set
            {
                if (_isReissue == value) return;
                _isReissue = value;
                RaisePropertyChanged(() => IsReissue);
            }
        }

        #endregion

        /// <summary>
        /// 仅召测显示的信息
        /// </summary>

        #region IsMeasure
        private bool _isMeasure;

        public bool IsMeasure
        {
            get { return _isMeasure; }
            set
            {
                if (_isMeasure == value) return;
                _isMeasure = value;
                RaisePropertyChanged(() => IsMeasure);
            }
        }

        #endregion

        /// <summary>
        /// Zgb地址
        /// </summary>

        #region ZgbAddress
        private int _zgbAddress;

        public int ZgbAddress
        {
            get { return _zgbAddress; }
            set
            {
                if (_zgbAddress == value) return;
                _zgbAddress = value;
                RaisePropertyChanged(() => ZgbAddress);
            }
        }

        #endregion

        /// <summary>
        /// 电流量程
        /// </summary>

        #region ARange
        private double _aRange;

        public double ARange
        {
            get { return _aRange; }
            set
            {
                if (_aRange == value) return;
                _aRange = value;
                RaisePropertyChanged(() => ARange);
            }
        }

        #endregion

        /// <summary>
        /// 功率量程
        /// </summary>

        #region PRange
        private int _pRange;

        public int PRange
        {
            get { return _pRange; }
            set
            {
                if (_pRange == value) return;
                _pRange = value;
                RaisePropertyChanged(() => PRange);
            }
        }

        #endregion


        /// <summary>
        /// 控制器数量
        /// </summary>

        #region ControlNum
        private int _controlNum;
        [Range( 1,255,ErrorMessage = "控制器数量在1到255之间")]
        public int ControlNum
        {
            get { return _controlNum; }
            set
            {
                if (_controlNum == value) return;
                _controlNum = value;
                RaisePropertyChanged(() => ControlNum);
            }
        }

        #endregion

        /// <summary>
        /// 域名
        /// </summary>

        #region DomainName
        private int _domainName;

        [Range(1, 16383, ErrorMessage = "域名介于1到16383之间")]
        public int DomainName
        {
            get { return _domainName; }
            set
            {
                if (_domainName == value) return;
                _domainName = value;
                RaisePropertyChanged(() => DomainName);
            }
        }

        #endregion


        /// <summary>
        /// 域名 lvf 2018年4月2日16:09:54  电力载波的域名
        /// </summary>

        #region DomainNamePower
        private int _domainNamePower;

        [Range(1, 6, ErrorMessage = "域名介于1到6之间")]
        public int DomainNamePower
        {
            get { return _domainNamePower; }
            set
            {
                if (_domainNamePower == value) return;
                if (value > 6) value = 6;
                if (value < 1) value = 1;
                _domainNamePower = value;
                RaisePropertyChanged(() => DomainNamePower);
            }
        }

        #endregion

        /// <summary>
        /// 未生效的新域名
        /// </summary>

        #region NoDomainName
        private int _noDomainName;

        public int NoDomainName
        {
            get { return _noDomainName; }
            set
            {
                if (_noDomainName == value) return;
                _noDomainName = value;
                RaisePropertyChanged(() => NoDomainName);
            }
        }

        #endregion

        /// <summary>
        /// 经度
        /// </summary>
        /// <summary>
        /// 经度
        /// </summary>

        #region Longitude
        private double _longitude;

        [Range(70.0, 150.99, ErrorMessage = "经度介于70.0到150.99")]
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude == value) return;
                _longitude = value;
                RaisePropertyChanged(() => Longitude);
            }
        }

        #endregion

        /// <summary>
        /// 纬度
        /// </summary>

        #region Latitude
        private double _latitude;

        /// <summary>
        /// 纬度
        /// </summary>
        [Range(10.0, 60.99, ErrorMessage = "纬度介于10.0到60.99")]
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude == value) return;
                _latitude = value;
                RaisePropertyChanged(() => Latitude);
            }
        }

        #endregion

        /// <summary>
        /// 通信信道
        /// </summary>

        #region IsCommunicationChannel
        private ObservableCollection<NameIntBool> _isCommunicationChannel;

        public ObservableCollection<NameIntBool> IsCommunicationChannel
        {
            get
            {
                if (_isCommunicationChannel == null)
                {
                    _isCommunicationChannel = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 16; i++)
                    {
                        _isCommunicationChannel.Add(
                            new NameIntBool() {IsSelected = false, Name = "IsCommunicationChannel", Value = i});
                    }

                }
                return _isCommunicationChannel;
            }
        }

        #endregion

        /// <summary>
        /// PIN码
        /// </summary>

        #region PCode
        private int _pCode;

         [Range(1, 100000000000000, ErrorMessage = "PIN码不超过15位数")]
        public int PCode
        {
            get { return _pCode; }
            set
            {
                if (_pCode == value) return;
                _pCode = value;
                RaisePropertyChanged(() => PCode);
            }
        }

        #endregion

        /// <summary>
        /// 蓝牙安全模式
        /// </summary>

        #region SafeMode
        private int _safeMode;

        public int SafeMode
        {
            get { return _safeMode; }
            set
            {
                if (_safeMode == value) return;
                _safeMode = value;
                RaisePropertyChanged(() => SafeMode);
            }
        }

        #endregion

        /// <summary>
        /// 路由运行模式
        /// </summary>

        #region RouteRunMode
        private int _routeRunMode;

        public int RouteRunMode
        {
            get { return _routeRunMode; }
            set
            {
                if (_routeRunMode == value) return;
                _routeRunMode = value;
                RaisePropertyChanged(() => RouteRunMode);
            }
        }

        #endregion

        /// <summary>
        /// 连续通讯失败报警
        /// </summary>

        #region CommunicationFailureNum
        private int _communicationFailureNum;

        public int CommunicationFailureNum
        {
            get { return _communicationFailureNum; }
            set
            {
                if (_communicationFailureNum == value) return;
                if (value > 50) value = 50;
                if (value < 1) value = 1;
                _communicationFailureNum = value;
                RaisePropertyChanged(() => CommunicationFailureNum);
            }
        }

        #endregion

        /// <summary>
        /// 电压报警上限
        /// </summary>

        #region VAlarmMax
        private int _vAlarmMax;

        public int VAlarmMax
        {
            get { return _vAlarmMax; }
            set
            {
                if (_vAlarmMax == value) return;
                if (value > 600) value = 600;
                if (value < 1) value = 1;
                _vAlarmMax = value;
                RaisePropertyChanged(() => VAlarmMax);
            }
        }

        #endregion

        /// <summary>
        /// 功率因数低报警
        /// </summary>

        #region PowerFactor
        private int _powerFactor;

        public int PowerFactor
        {
            get { return _powerFactor; }
            set
            {

                if (_powerFactor == value) return;
                if (value < 40) value = 40;
                if (value > 100) value = 100;
                _powerFactor = value;
                RaisePropertyChanged(() => PowerFactor);
            }
        }

        #endregion

        /// <summary>
        /// 电压报警下限
        /// </summary>

        #region VAlarmMin
        private int _vAlarmMin;

        public int VAlarmMin
        {
            get { return _vAlarmMin; }
            set
            {
                if (_vAlarmMin == value) return;
                if (value > 600) value = 600;
                if (value < 1) value = 1;
                _vAlarmMin = value;
                RaisePropertyChanged(() => VAlarmMin);
            }
        }

        #endregion


        /// <summary>
        /// 控制器24小时带电，0-不带电（集中器不转发时间），1-带电（集中器转发时间）
        /// </summary>

        #region AlwaysOnline
        private int _alwaysOnline;

        public int AlwaysOnline
        {
            get { return _alwaysOnline; }
            set
            {
                if (_alwaysOnline == value) return;
                _alwaysOnline = value;
                RaisePropertyChanged(() => AlwaysOnline);
            }
        }

        #endregion


        /// <summary>
        /// 功率调节
        /// </summary>

        #region PowerControl
        private bool _powerControl;

        public bool PowerControl
        {
            get { return _powerControl; }
            set
            {
                if (_powerControl == value) return;
                _powerControl = value;
                if (_powerControl)
                {
                    IsBaud = true;
                    IsFrequency = false;
                }
                else
                {
                    IsBaud = false;
                    IsFrequency = true;
                }
                RaisePropertyChanged(() => PowerControl);
            }
        }

        #endregion

        /// <summary>
        /// 波特率是否可用
        /// </summary>

        #region IsBaud
        private bool _isBaud;

        public bool IsBaud
        {
            get { return _isBaud; }
            set
            {
                if (_isBaud == value) return;
                _isBaud = value;
                RaisePropertyChanged(() => IsBaud);
            }
        }

        #endregion

        /// <summary>
        /// 频率是否可用
        /// </summary>

        #region IsFrequency
        private bool _isFrequency;

        public bool IsFrequency
        {
            get { return _isFrequency; }
            set
            {
                if (_isFrequency == value) return;
                _isFrequency = value;
                RaisePropertyChanged(() => IsFrequency);
            }
        }

        #endregion

        /// <summary>
        /// 频率
        /// </summary>

        #region ScanMode
        private bool _isScanMode;

        public bool ScanMode
        {
            get { return _isScanMode; }
            set
            {
                if (_isScanMode == value) return;
                _isScanMode = value;
                RaisePropertyChanged(() => ScanMode);
            }
        }

        #endregion

[Range(1, 10000.99, ErrorMessage = "调光频率大小介于1至10000之间")]
        #region Frequency
        private int _frequency;

        public int Frequency
        {
            get { return _frequency; }
            set
            {
                if (_frequency == value) return;
                _frequency = value;
                RaisePropertyChanged(() => Frequency);
            }
        }

        #endregion

        /// <summary>
        /// 波特率
        /// </summary>

        #region BaudItems
        private ObservableCollection<NameValueInt> _baudItems = null;

        public ObservableCollection<NameValueInt> BaudItems
        {
            get
            {
                if (_baudItems == null)
                {
                    _baudItems = new ObservableCollection<NameValueInt>();
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 600});
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 1200});
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 2400});
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 4800});
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 9600});
                    _baudItems.Add(new NameValueInt() {Name = "baud", Value = 19200});
                    CurrentBaudItem = _baudItems[4];
                }
                return _baudItems;
            }
        }

        #endregion

        #region CurrentBaudItem

        private NameValueInt _currentBaudItem;

        public NameValueInt CurrentBaudItem
        {
            get { return _currentBaudItem; }
            set
            {
                if (_currentBaudItem == value) return;
                _currentBaudItem = value;
                RaisePropertyChanged(() => CurrentBaudItem);
            }
        }

        #endregion

        /// <summary>
        /// 手机号码
        /// </summary>

        [StringLength(11, ErrorMessage = "手机号码长度不能大于11")]
        [Required(ErrorMessage = "输入不得为空")]
        #region PhoneNum
        private string _phoneNum;

        public string PhoneNum
        {
            get { return _phoneNum; }
            set
            {
                if (_phoneNum == value) return;
                _phoneNum = value;
                RaisePropertyChanged(() => PhoneNum);
            }
        }

        #endregion

        /// <summary>
        /// IP地址
        /// </summary>


        #region IPAdress
        private string _iPAdress;
        [RegularExpression(@"^(2[0-4]\d|25[0-5]|[01]?\d\d?\.){3}2[0-4]\d|25[0-5]|[01]?\d\d?$", ErrorMessage = "请输入正确的IP地址格式")]
        
        
        public string IPAdress
        {
            get { return _iPAdress; }
            set
            {
                if (_iPAdress == value) return;
                _iPAdress = value;
                RaisePropertyChanged(() => IPAdress);
            }
        }

        #endregion

        /// <summary>
        /// 安装日期
        /// </summary>

        #region InstallTime
        private DateTime _installTime;

        public DateTime InstallTime
        {
            get { return _installTime; }
            set
            {
                if (_installTime == value) return;
                _installTime = value;
                RaisePropertyChanged(() => InstallTime);
            }
        }

        #endregion

        /// <summary>
        /// 备注
        /// </summary>

        #region Remark
        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark == value) return;
                _remark = value;
                RaisePropertyChanged(() => Remark);
            }
        }

        #endregion


        #region SndSluParaVisi
        private Visibility _reSndSluParaVisimark;

        public Visibility SndSluParaVisi
        {
            get { return _reSndSluParaVisimark; }
            set
            {
                if (_reSndSluParaVisimark == value) return;
                _reSndSluParaVisimark = value;
                RaisePropertyChanged(() => SndSluParaVisi);
            }
        }

        #endregion


        #region SndCtrlParaVisi
        private Visibility _reSSndCtrlParaVisimark;

        public Visibility SndCtrlParaVisi
        {
            get { return _reSSndCtrlParaVisimark; }
            set
            {
                if (_reSSndCtrlParaVisimark == value) return;
                _reSSndCtrlParaVisimark = value;
                RaisePropertyChanged(() => SndCtrlParaVisi);
            }
        }

        #endregion
        
        #region CmdUp

        private ICommand _CmdUp;

        public ICommand CmdUp
        {
            get { return _CmdUp ?? (_CmdUp = new RelayCommand(ExCmdUp, CanCmdUp, false)); }
        }

        private void ExCmdUp()
        {
            ParaType -= 1; // = EnumParaTypes.ControlPara;
            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(ViewIdAssign.ControlInfoSetViewId, ViewIdAssign.ControlInfoSetViewAttachRegion,1);
        }

        private bool CanCmdUp()
        {
            return ParaType > 1 && ParaType < 5;
        }

        #endregion

        #region CmdNext

        private ICommand _cmdNext;

        public ICommand CmdNext
        {
            get { return _cmdNext ?? (_cmdNext = new RelayCommand(ExNext, CanNext, false)); }
        }

        private void ExNext()
        {
            
            ParaType += 1; // = EnumParaTypes.ControlPara;
            if (ParaType == 2)
            {
                InitControlViewModel();
            }
            if (ParaType == 3 && grpFlag)
            {
                InitGroupViewModel();
            }
            if (ParaType == 4)
            {
                ShowSndInfo = "";
                ShowSndInfoAll = "";
                ShowSndInfo1 = "";
                ShowSndInfo2 = "";
                ShowSndInfo3 = "";
                ShowSndInfo4 = "";
            }
            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(ViewIdAssign.ControlInfoSetViewId, ViewIdAssign.ControlInfoSetViewAttachRegion,1);
        }

        private bool CanNext()
        {
            return ParaType > 0 && ParaType < 4;
        }

        #endregion

        private bool existBarCode = false;
        private bool grpFlag;

        #region CmdSaveAndSnd

        private ICommand _CmdSaveAndSnd;

        public ICommand CmdSaveAndSnd
        {
            get { return _CmdSaveAndSnd ?? (_CmdSaveAndSnd = new RelayCommand(ExCmdSaveAndSnd, CanCmdSaveAndSnd, false)); }
        }

        private long dtsnd = 0;

        private void ExCmdSaveAndSnd()
        {
            var cansavll = CanSaveAll();
            if (cansavll == false) return;

            dtsnd = DateTime.Now.Ticks;
            var sluctrl = this.BackControlViewModelSluCtrl();
            if (existBarCode || sluctrl == null)
            {
                ParaType = 2;
                return;
            }
 
            if(RelatedRtuId == 0)
            {
                if (
                       Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                           "该集中器未绑定终端，是否需要绑定" + PhyId+"号终端", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    RelatedRtuId = 0;
                }else
                {
                    RelatedRtuId = PhyId;
                }
            }


            var para = new Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu(BackConcentratorViewModelEqu() , this.BackConcentratorViewModelSlu(),
                                                                        sluctrl,this .BackGroupViewModelSluCtrlgrp() );



            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(para );

            //ParaType = 4;
            IsEnableCore = false;
        }


        private bool CanCmdSaveAndSnd()
        {
            if (ParaType == 4)
            {
                return false;
            }
            return DateTime.Now.Ticks - dtsnd > 30000000;
        }

        #endregion


        #region CmdSndSluPara

        private ICommand _cmdNexCmdSndSluParat;

        public ICommand CmdSndSluPara
        {
            get
            {
                return _cmdNexCmdSndSluParat ??
                       (_cmdNexCmdSndSluParat = new RelayCommand(ExCmdSndSluPara, CanCmdSndSluPara, true));
            }
        }

        private long dtsndslu = DateTime.Now.AddDays(-1).Ticks;

        private void ExCmdSndSluPara()
        {
            dtsndslu = DateTime.Now.Ticks;

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_snd_order ;//.wls_cnt_wj2090_snd_slu_and_ctrl_pars;
            info.WstSluSndOrder.SluId = _terminalInformation.RtuId;

            info.Args.Addr.Add(_terminalInformation.RtuId);
            info.Args.Cid = 1;
            SndOrderServer.OrderSnd(info, 10, 6);

        }

        private bool CanCmdSndSluPara()
        {
            if (DateTime.Now.Ticks - dtsndslu > 10*10000000 && _terminalInformation != null) return true;
            return false;
        }

        #endregion


        #region CmdSndCtrlPara

        private ICommand _cmdNeCmdSndCtrlPara;

        public ICommand CmdSndCtrlPara
        {
            get
            {
                return _cmdNeCmdSndCtrlPara ??
                       (_cmdNeCmdSndCtrlPara = new RelayCommand(Ex_cmdNeCmdSndCtrlPara, Can_cmdNeCmdSndCtrlPara, true));
            }
        }

        private long dtsndctrl = DateTime.Now.AddDays(-1).Ticks;

        private void Ex_cmdNeCmdSndCtrlPara()
        {

            dtsndctrl = DateTime.Now.Ticks;

            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_snd_order ;//.wls_cnt_wj2090_snd_slu_and_ctrl_pars;
            //info.Args.Addr.Add(_terminalInformation.RtuId);
            info.WstSluSndOrder.SluId = _terminalInformation.RtuId;
            bool nochecked = true;
             foreach (var tt in ControlParaItems)
            {
                if (tt.IsChecked)
                {
                    nochecked = false;
                   // info.Args.Addr.Add(tt.RtuId);// tt.IsChecked = false;
                    info.WstSluSndOrder.CtrlIds.Add(tt.RtuId);
                }
            }
            if(nochecked )
            {
                WlstMessageBox.Show("无控制器选择", "请勾选控制器，然后下发参数", WlstMessageBoxType.Ok);

                return;
            }
            
            //info.Args.Cid = 2;
            SndOrderServer.OrderSnd(info, 10, 6);

        }

        private bool Can_cmdNeCmdSndCtrlPara()
        {
            if (ControlParaItems == null) return false;
            int rf = (from t in ControlParaItems where t.IsChecked select t).Count();
            if (rf == 0) return false;
            if (DateTime.Now.Ticks - dtsndctrl > 10*10000000 && _terminalInformation != null) return true;
            return false;
        }

        #endregion

    }

    /// <summary>
    /// 控制器属性
    /// </summary>
    public partial class ConcentratorParaInformationViewModel
    {
        private void InitControlViewModel()
        {
            BtnName = ">>>";
            FlagVisi = false;
            StartCtrl = 1;
            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }
            ControlParaItems.Clear();

            int xCount = this.ControlNum;
            int yCount = _terminalInformation.WjSluCtrls .Count;
            if (xCount <= yCount)
            {
                var tmps = (from t in _terminalInformation.WjSluCtrls  orderby t.Value .CtrlPhyId ascending select t.Value ).ToList();
                for (int i = 0; i < xCount; i++)
                {
                    AddToItems(tmps[i]);
                }

            }
            else
            {
                var tmps = (from t in _terminalInformation.WjSluCtrls  orderby t.Value .CtrlPhyId ascending select t.Value ).ToList();
                foreach (var g in tmps)
                {
                    AddToItems(g);
                }
                int max = 0;
                foreach (var g in _terminalInformation.WjSluCtrls.Values  )
                {
                    if (g.CtrlId > max) max = g.CtrlId;
                }
                max++;
                int index0 = _terminalInformation.WjSluCtrls .Count;
                index0++;
                int xAdd = xCount - yCount;

                for (int i = max; i < max + xAdd; i++)
                {
                    var ntps = new ControlParaItem
                                   {
                                       RtuId = i,
                                       Index = 0,
                                       LightIndex = ControlParaItems.Count +1,
                                       BarCode = i.ToString().PadLeft(13, '0'),
                                       //"000 000 000 0000",
                                       IsActiveAlarm = true,
                                       IsRun = true,
                                       PowerMax = 120,
                                       PowerMin = 80,
                                       IsPowerOnLight1 = true,
                                       IsPowerOnLight2 = true,
                                       IsPowerOnLight3 = true,
                                       IsPowerOnLight4 = true,
                                       LampCode =i +""
                                   };

                    //ntps.CurrentSelectLoopNumItem = ntps.LoopNumItems[0];
                    //ntps.IsPowerOnLight1 = true;
                    //ntps.IsPowerOnLight2 = true;
                    //ntps.IsPowerOnLight3 = true;
                    //ntps.IsPowerOnLight4 = true;


                    //ntps.CurrentSelectLoopRatePowerIndex1 = ntps.LoopRatePowerItems1[8];
                    //ntps.CurrentSelectLoopRatePowerIndex2 = ntps.LoopRatePowerItems2[8];
                    //ntps.CurrentSelectLoopRatePowerIndex3 = ntps.LoopRatePowerItems3[8];
                    //ntps.CurrentSelectLoopRatePowerIndex4 = ntps.LoopRatePowerItems4[8];



                    //ntps.CurrentSelectLoopVectorItem1 = ntps.LoopVectorItems1[0];
                    //ntps.CurrentSelectLoopVectorItem2 = ntps.LoopVectorItems2[1];

                    //ntps.CurrentSelectLoopVectorItem3 = ntps.LoopVectorItems3[2];

                    //ntps.CurrentSelectLoopVectorItem4 = ntps.LoopVectorItems4[3];

                    ControlParaItems.Add(ntps);
                    index0++;
                    //  max++;
                }
            }
            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
            EndCtrl = ControlParaItems.Count;


            foreach (var g in this.ControlParaItems )
            {
                g.OnAttriChanged += new EventHandler<AttriChangedArgs>(g_OnAttriChanged);
            }
        }

        private void g_OnAttriChanged(object sender, AttriChangedArgs e)
        {
            //throw new NotImplementedException();

            var changedItem = sender as ControlParaItem;
            if (changedItem == null) return;
            foreach (var g in ControlParaItems)
            {
                if (g.IsChecked == false) continue;
                switch (e.AttriIndex)
                {
                    case 1:
                        g.IsActiveAlarm = changedItem.IsActiveAlarm;
                        break;
                    case 2:
                        g.IsRun = changedItem.IsRun;
                        break;
                    case 3:
                        foreach (var t in g.LoopNumItems)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopNumItem.Value)
                                g.CurrentSelectLoopNumItem = t;
                        }
                        break;
                    case 4:
                        g.IsPowerOnLight1 = changedItem.IsPowerOnLight1;
                        break;
                    case 5:
                        g.IsPowerOnLight2 = changedItem.IsPowerOnLight2;
                        break;
                    case 6:
                        g.IsPowerOnLight3 = changedItem.IsPowerOnLight3;
                        break;
                    case 7:
                        g.IsPowerOnLight4 = changedItem.IsPowerOnLight4;
                        break;
                    case 8:
                        foreach (var t in g.LoopVectorItems1)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem1.Value)
                                g.CurrentSelectLoopVectorItem1 = t;
                        }
                        break;
                    case 9:
                        foreach (var t in g.LoopVectorItems2)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem2.Value)
                                g.CurrentSelectLoopVectorItem2 = t;
                        }
                        break;
                    case 10:
                        foreach (var t in g.LoopVectorItems3)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem3.Value)
                                g.CurrentSelectLoopVectorItem3 = t;
                        }
                        break;
                    case 11:
                        foreach (var t in g.LoopVectorItems4)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem4.Value)
                                g.CurrentSelectLoopVectorItem4 = t;
                        }
                        break;
                    case 12:
                        foreach (var t in g.LoopRatePowerItems1)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex1.Value)
                                g.CurrentSelectLoopRatePowerIndex1 = t;
                        }
                        break;
                    case 13:
                        foreach (var t in g.LoopRatePowerItems2)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex2.Value)
                                g.CurrentSelectLoopRatePowerIndex2 = t;
                        }
                        break;
                    case 14:
                        foreach (var t in g.LoopRatePowerItems3)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex3.Value)
                                g.CurrentSelectLoopRatePowerIndex3 = t;
                        }
                        break;
                    case 15:
                        foreach (var t in g.LoopRatePowerItems4)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex4.Value)
                                g.CurrentSelectLoopRatePowerIndex4 = t;
                        }
                        break;
                    case 16:
                        g.PowerMax = changedItem.PowerMax;
                        break;
                    case 17:
                        g.PowerMin = changedItem.PowerMin;
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddToItems(SluRegulatorParameter   tmps)
        {

            var gf = new ControlParaItem()
                         {
                             RtuId = tmps.CtrlId,
                             Index = tmps.CtrlPhyId,
                             BarCodeId = tmps.BarCodeId,
                             IsActiveAlarm = tmps.IsAlarmAuto,
                             IsRun = tmps.IsUsed,
                             PowerMax = tmps.UpperPower,
                             PowerMin = tmps.LowerPower,
                             Route1 = tmps.RoutePass1,
                             Route2 = tmps.RoutePass2,
                             Route3 = tmps.RoutePass3,
                             Route4 = tmps.RoutePass4,
                             LampCode = tmps.LampCode,
                             LightIndex = tmps.OrderId,
                             Xgis =tmps .CtrlGisX  ,
                             Ygis =tmps .CtrlGisY  

                         };
            gf.BarCode = string.Format("{0:D13}", gf.BarCodeId);
            for (int i = 0; i < 3; i++)
            {
                gf.BarCode = gf.BarCode.Insert(4 * i + 3, " ");
            }

            foreach (var g in gf.LoopRatePowerItems1)
            {
                if (g.Value == tmps.PowerRate1)
                {
                    gf.CurrentSelectLoopRatePowerIndex1 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems2)
            {
                if (g.Value == tmps.PowerRate2)
                {
                    gf.CurrentSelectLoopRatePowerIndex2 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems3)
            {
                if (g.Value == tmps.PowerRate3)
                {
                    gf.CurrentSelectLoopRatePowerIndex3 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems4)
            {
                if (g.Value == tmps.PowerRate4)
                {
                    gf.CurrentSelectLoopRatePowerIndex4 = g;
                    break;
                }
            }



            foreach (var g in gf.LoopVectorItems1)
            {
                if (g.Value == tmps.VectorLoop1)
                {
                    gf.CurrentSelectLoopVectorItem1 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems2)
            {
                if (g.Value == tmps.VectorLoop2)
                {
                    gf.CurrentSelectLoopVectorItem2 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems3)
            {
                if (g.Value == tmps.VectorLoop3)
                {
                    gf.CurrentSelectLoopVectorItem3 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems4)
            {
                if (g.Value == tmps.VectorLoop4)
                {
                    gf.CurrentSelectLoopVectorItem4 = g;
                    break;
                }
            }

            gf.IsPowerOnLight1 = tmps.IsAutoOpenLightWhenElec1;
            gf.IsPowerOnLight2 = tmps.IsAutoOpenLightWhenElec2;
            gf.IsPowerOnLight3 = tmps.IsAutoOpenLightWhenElec3;
            gf.IsPowerOnLight4 = tmps.IsAutoOpenLightWhenElec4;

            foreach (var g in gf.LoopNumItems)
            {
                if (g.Value == tmps.LightCount)
                {
                    gf.CurrentSelectLoopNumItem = g;
                    break;
                }
            }
            ControlParaItems.Add(gf);

        }
        /// <summary>
        /// 系统下 所有控制器条形码
        /// </summary>
        private ConcurrentDictionary<int, List<long>> allctrllist = new ConcurrentDictionary<int, List<long>>();

        public List<SluRegulatorParameter> BackControlViewModelSluCtrl()
        {

            var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Slu);
            allctrllist.Clear();
            foreach (var j in sluinfo)
            {
                var t =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[j.RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (t == null) continue;
                if (allctrllist.ContainsKey(j.RtuId) == false)
                {
                    allctrllist.TryAdd(j.RtuId, new List<long>());
                }
                else
                {
                    allctrllist[j.RtuId] = new List<long>();
                }
                //lvf 判断控制器条形码 是否重复
                foreach (var g in t.WjSluCtrls)
                {
                    allctrllist[j.RtuId].Add(g.Value.BarCodeId);
                }
              
            }


            string existInfo = null;
            existBarCode = false;
            var lst = new List<SluRegulatorParameter>();
            for (int i = 0; i < ControlParaItems.Count; i++)
            {
                for (int j = i + 1; j < ControlParaItems.Count; j++)
                {
                    if (ControlParaItems[i].BarCodeId == ControlParaItems[j].BarCodeId)
                    {
                        existBarCode = true;
                        existInfo = "第" + ControlParaItems[i].Index + "控制器与第" + ControlParaItems[j].Index + "控制器条形码相同，请重新设置";
                        Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("有条形码相同的控制器", existInfo, UMessageBoxButton.Ok);
                        return null ;
                    }
                   


                }
                //foreach (var g in allctrllist)
                //{
                //    //如果列表中存在该条形码 且 是该集中器，则通过 否则
                //    if (g.Value.Contains(ControlParaItems[i].BarCodeId))
                //    {
                //        if (g.Key != _terminalInformation.RtuId)
                //        {
                //            var phyiddd =
                //                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.Key).RtuPhyId;
                //            Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                //          "条形码有重复。重复条形码为 " + ControlParaItems[i].BarCodeId + "集中器" + phyiddd + "下有该条形码", WlstMessageBoxType.Ok);
                //            return null;
                //        }

                //    }
                //}

            }
           
            foreach (var g in this.ControlParaItems)
                {
                    var info = new SluRegulatorParameter()
                                                          {
                                                              BarCodeId = g.BarCodeId,
                                                              OrderId = g.LightIndex,
                                                              CtrlId = g.RtuId,
                                                              CtrlPhyId = g.Index,
                                                              LampCode = g.LampCode,
                                                              IsAlarmAuto = g.IsActiveAlarm,
                                                              IsUsed = g.IsRun,
                                                              LightCount =
                                                                  g.CurrentSelectLoopNumItem == null
                                                                      ? 0
                                                                      : g.CurrentSelectLoopNumItem.Value,
                                                              IsAutoOpenLightWhenElec1 = g.IsPowerOnLight1,
                                                              IsAutoOpenLightWhenElec2 = g.IsPowerOnLight2,
                                                              IsAutoOpenLightWhenElec3 = g.IsPowerOnLight3,
                                                              IsAutoOpenLightWhenElec4 = g.IsPowerOnLight4,
                                                              VectorLoop1 =
                                                                  g.CurrentSelectLoopVectorItem1 == null
                                                                      ? 1
                                                                      : g.CurrentSelectLoopVectorItem1.Value,
                                                              VectorLoop2 =
                                                                  g.CurrentSelectLoopVectorItem2 == null
                                                                      ? 2
                                                                      : g.CurrentSelectLoopVectorItem2.Value,
                                                              VectorLoop3 =
                                                                  g.CurrentSelectLoopVectorItem3 == null
                                                                      ? 3
                                                                      : g.CurrentSelectLoopVectorItem3.Value,
                                                              VectorLoop4 =
                                                                  g.CurrentSelectLoopVectorItem4 == null
                                                                      ? 4
                                                                      : g.CurrentSelectLoopVectorItem4.Value,
                                                              PowerRate1 =
                                                                  g.CurrentSelectLoopRatePowerIndex1 == null
                                                                      ? 0
                                                                      : g.CurrentSelectLoopRatePowerIndex1.Value,
                                                              PowerRate2 =
                                                                  g.CurrentSelectLoopRatePowerIndex2 == null
                                                                      ? 0
                                                                      : g.CurrentSelectLoopRatePowerIndex2.Value,
                                                              PowerRate3 =
                                                                  g.CurrentSelectLoopRatePowerIndex3 == null
                                                                      ? 0
                                                                      : g.CurrentSelectLoopRatePowerIndex3.Value,
                                                              PowerRate4 =
                                                                  g.CurrentSelectLoopRatePowerIndex4 == null
                                                                      ? 0
                                                                      : g.CurrentSelectLoopRatePowerIndex4.Value,
                                                              RoutePass1 = g.Route1,
                                                              RtuName = "控制器" + g.RtuId,
                                                              RoutePass2 = g.Route2,
                                                              RoutePass3 = g.Route3,
                                                              RoutePass4 = g.Route4,
                                                              LowerPower = g.PowerMin,
                                                              SluId = _terminalInformation.RtuId,
                                                              UpperPower = g.PowerMax,
                                                              CtrlGisX  =g.Xgis ,
                                                              CtrlGisY  =g.Ygis 
                                                          };
                    lst .Add(  info);
                }
            return lst;
        }
    }

    public partial class ConcentratorParaInformationViewModel 
    {
        #region ControlParaItems

        private ObservableCollection<ControlParaItem> _controlParaItems;

        public ObservableCollection<ControlParaItem> ControlParaItems
        {
            get { return _controlParaItems ?? (_controlParaItems = new ObservableCollection<ControlParaItem>()); }
        }

        #endregion

        #region CurrentSelectControlParaItem

        private ControlParaItem _currentSelectControlParaItem;

        public ControlParaItem CurrentSelectControlParaItem
        {
            get { return _currentSelectControlParaItem; }
            set
            {
                if (_currentSelectControlParaItem == value) return;
                _currentSelectControlParaItem = value;
                RaisePropertyChanged(() => CurrentSelectControlParaItem);
              //  _currentSelectControlParaItem = null;
            }
        }

        #endregion



        #region UpMove

        private ICommand _CUpMovemdUp;

        public ICommand UpMove
        {
            get { return _CUpMovemdUp ?? (_CUpMovemdUp = new RelayCommand(ExUpMove, CanUpMove, false)); }
        }

        private void ExUpMove()
        {

            for (int i=1;i<ControlParaItems .Count ;i++)
            {
                if(ControlParaItems [i].IsChecked && ControlParaItems [i-1].IsChecked ==false )
                {
                    var tmpg = ControlParaItems[i-1];
                    ControlParaItems .RemoveAt(i-1);
                    ControlParaItems .Insert(i,tmpg );
                }
            } for (int i = 1; i < ControlParaItems.Count + 1; i++)
            {
                ControlParaItems[i - 1].Index = i;
                //ControlParaItems[i - 1].LightIndex  = i;
            }
        }

        private bool CanUpMove()
        {

            if (ControlParaItems.Count == 0) return false;
            if (ControlParaItems[0].IsChecked) return false;
            foreach (var g in ControlParaItems) if (g.IsChecked) return true;
            return false;
        }

        #endregion

        #region DownMove

        private ICommand _cmdDownMove;

        public ICommand DownMove
        {
            get { return _cmdDownMove ?? (_cmdDownMove = new RelayCommand(ExDownMove, CanDownMove, false)); }
        }

        private void ExDownMove()
        {
            for (int i = ControlParaItems.Count - 2; i >= 0; i--)
            {
                if (ControlParaItems[i].IsChecked && ControlParaItems[i + 1].IsChecked == false)
                {
                    var tmpg = ControlParaItems[i ];
                    ControlParaItems.RemoveAt(i );
                    ControlParaItems.Insert(i+1, tmpg);
                }
            } 
            for (int i = 1; i < ControlParaItems.Count + 1; i++)
            {
                ControlParaItems[i - 1].Index = i;
                //ControlParaItems[i - 1].LightIndex  = i;
            }
            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(ViewIdAssign.ControlInfoSetViewId, ViewIdAssign.ControlInfoSetViewAttachRegion,1);
        }

        private bool CanDownMove()
        {
            if (ControlParaItems.Count == 0) return false;
            if (ControlParaItems[ControlParaItems.Count - 1].IsChecked) return false;
            foreach (var g in ControlParaItems) if (g.IsChecked) return true;
            return false;
        }

        #endregion


        #region DeleteItem

        private ICommand _cmdDeleteItem;

        public ICommand DeleteItem
        {
            get { return _cmdDeleteItem ?? (_cmdDeleteItem = new RelayCommand(Ex_cmdDeleteItem, Can_cmdDeleteItem, false)); }
        }

        private void Ex_cmdDeleteItem()
        {
            if (CurrentSelectControlParaItem == null ||CurrentSelectControlParaItem .IsChecked ==false ) return;
            if(ControlParaItems.Contains(CurrentSelectControlParaItem ))
            {
                ControlParaItems.Remove(CurrentSelectControlParaItem);
                CurrentSelectControlParaItem = null;

                for (int i = 1; i < ControlParaItems.Count + 1;i++ )
                {
                    ControlParaItems[i - 1].Index = i;
                    //ControlParaItems[i - 1].LightIndex  = i;
                }
                if (ControlNum > 1) ControlNum -= 1;
            }
        }

        private bool Can_cmdDeleteItem()
        {
            return CurrentSelectControlParaItem != null && CurrentSelectControlParaItem .IsChecked ;
        }

        #endregion


        private int _startCtrl;
        /// <summary>
        /// 起始控制器
        /// </summary>
        [Required(ErrorMessage ="必填选项")]
        [Range(1,255,ErrorMessage="起始控制器在1-255号之间" )]
        public int StartCtrl
        {
            get { return _startCtrl; }
            set
            {
                if (_startCtrl != value)
                {
                    _startCtrl = value;
                    this.RaisePropertyChanged(() => this.StartCtrl);
                }
                if (StartCtrl < 1) return;
            }
        }

        private int _endCtrl;
        /// <summary>
        /// 结束控制器
        /// </summary>
        [Required(ErrorMessage = "必填选项")]
        [Range(1, 255, ErrorMessage = "结束控制器在1-255号之间")]
        public int EndCtrl
        {
            get { return _endCtrl; }
            set
            {
                if (_endCtrl != value)
                {

                    if (value > ControlParaItems.Count)
                        value = ControlParaItems.Count;
                    _endCtrl = value;
                    this.RaisePropertyChanged(() => this.EndCtrl);

                }
            }
        }

        #region CmdCtrlSelected

        private ICommand _cmdCtrlSelected;

        public ICommand CmdCtrlSelected
        {
            get { return _cmdCtrlSelected ?? (_cmdCtrlSelected = new RelayCommand(ExCtrlSelected, CanCtrlSelected, false)); }
        }

        private void ExCtrlSelected()
        {
            foreach (var t in ControlParaItems)
            {
                t.IsChecked = false;
            }
            for (int i = StartCtrl - 1; i < EndCtrl; i++)
            {
                ControlParaItems[i].IsChecked = true;
            }
        }

        private bool CanCtrlSelected()
        {
            return true;
        }

        #endregion

        #region CmdCtrlEnd

        private ICommand _cmdCtrlEnd;

        public ICommand CmdCtrlEnd
        {
            get { return _cmdCtrlEnd ?? (_cmdCtrlEnd = new RelayCommand(ExCtrlEnd, CanCtrlEnd, false)); }
        }

        private void ExCtrlEnd()
        {
            foreach (var t in ControlParaItems)
            {
                if (t.IsChecked)
                {
                    t.IsChecked = false;
                    //CurrentSelectControlParaItem = null;
                }

            }
        }

        private bool CanCtrlEnd()
        {
            return true;
        }

        #endregion

        #region CmdCtrlDelete

        private ICommand _cmdCtrlDelete;

        public ICommand CmdCtrlDelete
        {
            get { return _cmdCtrlDelete ?? (_cmdCtrlDelete = new RelayCommand(ExCtrlDelete, CanCtrlDelete, false)); }
        }

        private void ExCtrlDelete()
        {
            if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要删除选中的控制器，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
            {
                return;
            }
            for (int i = 0; i < ControlParaItems.Count; i++)
            {
                if (ControlParaItems[i].IsChecked)
                {
                    try
                    {
                        ControlParaItems[i].OnAttriChanged -= this.g_OnAttriChanged;
                    }catch (Exception ex)
                    {
                        
                    }
                    ControlParaItems.RemoveAt(i);
                    ControlNum = ControlNum - 1;
                    i--;
                }
            }
            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
        }

        private bool CanCtrlDelete()
        {
            return true;
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

        #region FlagVisi

        private bool _flagVisi;

        public bool FlagVisi
        {
            get { return _flagVisi; }
            set
            {
                if (_flagVisi == value) return;
                _flagVisi = value;
                RaisePropertyChanged(() => FlagVisi);
            }
        }

        #endregion

        #region CmdStretch

        private ICommand _cmdStretch;

        public ICommand CmdStretch
        {
            get { return _cmdStretch ?? (_cmdStretch = new RelayCommand(ExStretch, CanStrech, false)); }
        }

        private void ExStretch()
        {
            if (!FlagVisi)
            {
                FlagVisi = true;
                BtnName = "<<<";
            }
            else
            {
                FlagVisi = false;
                BtnName = ">>>";
            }
        }

        private bool CanStrech()
        {
            return true;
        }

        #endregion
    }

    public partial class ConcentratorParaInformationViewModel
    {
        private Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu _terminalInformation = null;

        private void SelectedSingleChange(int singleId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                     ContainsKey(singleId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [singleId]
                as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;

            if (t == null)
                return;
 
            _terminalInformation = t ;

            InitConcentratorViewModel();
            InitControlViewModel();
            InitGroupViewModel();            
        }




    }


    public partial class ConcentratorParaInformationViewModel
    {
        private ObservableCollection<NameIntBool> _isPowerOnLight;

        public ObservableCollection<NameIntBool> SndSelectedItems
        {
            get
            {
                if (_isPowerOnLight == null)
                {
                    _isPowerOnLight = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i < 9; i++)
                    {
                        _isPowerOnLight.Add(
                            new NameIntBool() {IsSelected = false, Name = "", Value = i});
                    }

                }
                return _isPowerOnLight;
            }
        }

        #region CmdZcOrSnd

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdZcOrSnd
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, true)); }
        }


        private long lastexute = 0;
        private int lastexutetpara = 0;

        private void ExCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            lastexute = DateTime.Now.Ticks;
            lastexutetpara = x;

            SndZcOrSndToSvr(x);

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x == 9 && IsZigbee == false) return false;
            if (x == 12 && IsZigbee == false) return false;
            if (x == lastexutetpara)
            {

                return DateTime.Now.Ticks - lastexute > 30000000;
            }
            return true;
            // return x != lastexutetpara && DateTime.Now.Ticks - lastexute > 30000000;
        }

        #endregion



        private static string[] StrNt = new string[16]
                                            {
                                                "复位网络1", "复位网络2", "复位网络3", "复位网络4", "设置集中器巡测", "设置集中器报警和投运停运", "设置集中器参数"
                                                ,
                                                "设置集中器报警参数", "设置控制器域名参数", "设置时钟",
                                                "召测集中器参数", "召测控制器域名修改信息", "召测集中器报警参数", "召测软件版本", "召测时钟", "复位及网络初始化"

                                            };

        private string _showSndInfo;

        public string ShowSndInfo
        {
            get { return _showSndInfo; }
            set
            {
                if (_showSndInfo == value) return;
                _showSndInfo = value;
                RaisePropertyChanged(() => ShowSndInfo);

            }
        }

        private string _showSndInfo1;

        public string ShowSndInfo1
        {
            get { return _showSndInfo1; }
            set
            {
                if (_showSndInfo1 == value) return;
                _showSndInfo1 = value;
                RaisePropertyChanged(() => ShowSndInfo1);
            }
        }

        private string _showSndInfo2;

        public string ShowSndInfo2
        {
            get { return _showSndInfo2; }
            set
            {
                if (_showSndInfo2 == value) return;
                _showSndInfo2 = value;
                RaisePropertyChanged(() => ShowSndInfo2);
            }
        }

        private string _showSndInfo3;

        public string ShowSndInfo3
        {
            get { return _showSndInfo3; }
            set
            {
                if (_showSndInfo3 == value) return;
                _showSndInfo3 = value;
                RaisePropertyChanged(() => ShowSndInfo3);
            }
        }

        private string _showSndInfo4;

        public string ShowSndInfo4
        {
            get { return _showSndInfo4; }
            set
            {
                if (_showSndInfo4 == value) return;
                _showSndInfo4 = value;
                RaisePropertyChanged(() => ShowSndInfo4);
            }
        }

        private string _showSndInfoall;

        public string ShowSndInfoAll
        {
            get { return _showSndInfoall; }
            set
            {
                if (_showSndInfoall == value) return;
                _showSndInfoall = value;
                RaisePropertyChanged(() => ShowSndInfoAll);
            }
        }

        private void SndZcOrSndToSvr(int x)
        {
            if (x < 1 || x > 16) return;
            // Msg = "snd  x=" + x;

            var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_zc_or_set;
                // .wlst_cnt_wj2090_set_or_zc_set;//.ServerPart.wlst_Wj2090_clinet_order_slu_zc_or_set;
            info.WstSluZcOrSet.Op = x;
            info.WstSluZcOrSet.SluId = SingleId;
            //info.WstCntOrderWj2090ZcOrSet.OrdId = x;
            if (x == 16)
            {
                info.WstSluZcOrSet.SetResetAndInit = new SluSetAndRead.ResetAndInit()
                                                         {
                                                             ClearArgs = SndSelectedItems[6].IsSelected,
                                                             ClearData = SndSelectedItems[5].IsSelected,
                                                             ClearTask = SndSelectedItems[7].IsSelected,
                                                             HardReZigbee = SndSelectedItems[1].IsSelected,
                                                             InitAll = SndSelectedItems[4].IsSelected,
                                                             ReCarrier = SndSelectedItems[3].IsSelected,
                                                             ReConcentrator = SndSelectedItems[0].IsSelected,
                                                             SluId = SingleId,
                                                             SoftReZigbee = SndSelectedItems[2].IsSelected
                                                         };
            }
            if (x == 7)
            {
                //th = new Thread(ExSnd);
                //th.Start();

                ShowSndInfoAll = " 正在发送集中器参数。";
                ShowSndInfo1 = "";
                ShowSndInfo2 = "";
                ShowSndInfo3 = "";
                ShowSndInfo4 = "";

                Wlst.Cr.Coreb.AsyncTask .Qtz .AddQtz("nuu", 8888, DateTime.Now.Ticks + 10000000, 0, ExSnd, null, 1);
            }
            else
            {
                SndOrderServer.OrderSnd(info);
                ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x - 1] + "..." +
                               " 集中器逻辑地址：" + SingleId +
                               Environment.NewLine;
            }

            //if (x == 7)
            //{

            //    ShowSndInfo += 1 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x - 3] + "..." + " 集中器逻辑地址：" + SingleId +
            //               Environment.NewLine;
            //    ShowSndInfo += 2 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x - 2] + "..." + " 集中器逻辑地址：" + SingleId +
            //               Environment.NewLine;
            //    ShowSndInfo += 3 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x - 1] + "..." + " 集中器逻辑地址：" + SingleId +
            //               Environment.NewLine;
            //    ShowSndInfo += 4 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x] + "..." + " 集中器逻辑地址：" + SingleId +
            //               Environment.NewLine;

            //}
            //else
            //    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[x - 1] + "..." + " 集中器逻辑地址：" + SingleId +
            //               Environment.NewLine;
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                SingleId, SingleName, OperatrType.UserOperator, StrNt[x - 1]);

        }


        private void ExSnd(object obj)
        {
            for (int i = 5; i < 9; i++)
            {
                var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_zc_or_set;
                // .wlst_cnt_wj2090_set_or_zc_set;//.ServerPart.wlst_Wj2090_clinet_order_slu_zc_or_set;
                info.WstSluZcOrSet.Op = 7;
                info.WstSluZcOrSet.SluId = SingleId;

                info.Head.Gid += 1;
                info.WstSluZcOrSet.Op = i;
                SndOrderServer.OrderSnd(info);
                ShowSndInfo += i - 4 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发送：" + StrNt[i - 1] +
                               "..." + " 集中器逻辑地址：" + SingleId +
                               Environment.NewLine;

                switch (i)
                {
                    case 5:
                        ShowSndInfoAll = " 已发送第一条集中器参数，共四条。";
                        ShowSndInfo1 = " 发送：设置集中器巡测                               --";
                        break;
                    case 6:
                        ShowSndInfoAll = " 已发送第二条集中器参数，共四条。";
                        ShowSndInfo2 = " 发送：设置集中器报警和投运停运              --";
                        break;
                    case 7:
                        ShowSndInfoAll = " 已发送第三条集中器参数，共四条。";
                        ShowSndInfo3 = " 发送：设置集中器参数                               --";
                        break;
                    case 8:
                        ShowSndInfoAll = " 已发送第四条集中器参数，共四条。";
                        ShowSndInfo4 = " 发送：设置集中器报警参数                        --";
                        break;
                }

                Thread.Sleep(4000);
            }

        }


        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_slu_zc_or_set,
                //.ClientPart.wlst_Wj2090_svr_ans_clinet_order_slu_zc_or_set , 
                OnZcOrSetBack,
                typeof (ConcentratorParaInformationViewModel), this);

        }


        private void OnZcOrSetBack(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstSluSvrAnsZcOrSet == null) return;
            if (info.WstSluSvrAnsZcOrSet.Op < 1 || info.WstSluSvrAnsZcOrSet.Op > 16) return;

            int sludid = info.WstSluSvrAnsZcOrSet.SluId;
            if (sludid != this.SingleId) return;

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                SingleId, SingleName, OperatrType.ServerReply, StrNt[info.WstSluSvrAnsZcOrSet.Op - 1]);
            if (isViewActive == false) return;
            string atttinfo = info.WstSluSvrAnsZcOrSet.IsSuccessfull ? "成功" : "失败";
            if (info.WstSluSvrAnsZcOrSet.IsSuccessfull == false)
            {
                atttinfo += "   " + info.WstSluSvrAnsZcOrSet.Info;
            }


            ShowSndInfo += "-----------------------------------------" +
                           Environment.NewLine;

            if (info.WstSluSvrAnsZcOrSet.Op < 5)
            {
                ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：复位网络  " + atttinfo +
                               Environment.NewLine;
                return;
            }
            if (info.WstSluSvrAnsZcOrSet.Op < 10)
            {

                ShowSndInfo += info.WstSluSvrAnsZcOrSet.Op - 4 + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                               " 接收：" + StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                               "  设置" + atttinfo +
                               Environment.NewLine;

                string res = atttinfo.Contains("成功") ? "√" : "╳";

                switch (info.WstSluSvrAnsZcOrSet.Op)
                {
                    case 5:
                        ShowSndInfo1 = " 发送：设置集中器巡测                               " + res;
                        break;
                    case 6:
                        ShowSndInfo2 = " 发送：设置集中器报警和投运停运              " + res;
                        break;
                    case 7:
                        ShowSndInfo3 = " 发送：设置集中器参数                               " + res;
                        break;
                    case 8:
                        ShowSndInfo4 = " 发送：设置集中器报警参数                        " + res;
                        break;
                }
            }
            if (info.WstSluSvrAnsZcOrSet.Op == 10)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  对时" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.TimeFault ? "失败" : "成功") +
                                   Environment.NewLine;
                    //  ShowSndInfo += "原始数据为：" + info.Data.ZcJzqTime.ToString();
                    ShowSndInfo += "时钟：" +
                                   new DateTime(info.WstSluSvrAnsZcOrSet.ZcJzqTime.DateTime).ToString(
                                       "yyyy-MM-dd HH:mm:ss") +
                                   Environment.NewLine;
                    ShowSndInfo += "强制对时：" + info.WstSluSvrAnsZcOrSet.ZcJzqTime.ForceTimer + Environment.NewLine;
                    ShowSndInfo += "时间数据错误：" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.DtformatError ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "对时失败：" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.TimeFault ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "时钟超差：" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.TimerError ? "是" : "否") +
                                   Environment.NewLine;
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 11)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  召测成功" +
                                   Environment.NewLine;
                    ShowSndInfo += "Zigebee地址：" + info.WstSluSvrAnsZcOrSet.ZcJzqPara.MacAddr + Environment.NewLine;
                    ShowSndInfo += "控制器数目：" + info.WstSluSvrAnsZcOrSet.ZcJzqPara.Ctrls + Environment.NewLine;
                    ShowSndInfo += "域名：" + info.WstSluSvrAnsZcOrSet.ZcJzqPara.DomainName + Environment.NewLine;
                    ShowSndInfo += "电压上限：" + info.WstSluSvrAnsZcOrSet.ZcJzqPara.UpperVoltageLimit + Environment.NewLine;
                    ShowSndInfo += "电压下限：" + info.WstSluSvrAnsZcOrSet.ZcJzqPara.LowerVoltageLimit + Environment.NewLine;
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 12)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  召测成功" +
                                   Environment.NewLine;
                    List<int> succ = new List<int>();
                    List<int> fail = new List<int>();
                    int onelent = 12;
                    for (int i = 1; i < info.WstSluSvrAnsZcOrSet.ZcCtrlDomainChangeInfo.Count + 1; i++)
                    {
                        if (info.WstSluSvrAnsZcOrSet.ZcCtrlDomainChangeInfo[i - 1]) succ.Add(i);
                        else fail.Add(i);
                    }
                    int x = 0;

                    string str = "设置成功：";
                    ShowSndInfo += "成功:" + succ.Count + "   失败:" + fail.Count + "   总数:" + (fail.Count + succ.Count) +
                                   Environment.NewLine;
                    if (succ.Count > 0)
                    {
                        foreach (var g in succ)
                        {
                            str += g + "-";
                            if (x >= onelent)
                            {
                                ShowSndInfo += str + Environment.NewLine;
                                str = "设置成功：";
                                x = 0;
                            }
                        }
                        ShowSndInfo += str + Environment.NewLine;
                    }

                    if (fail.Count > 0)
                    {
                        x = 0;
                        str = "设置失败：";
                        foreach (var g in fail)
                        {
                            str += g + "-";
                            if (x >= onelent)
                            {
                                ShowSndInfo += str + Environment.NewLine;
                                str = "设置失败：";
                                x = 0;
                            }
                        }
                        ShowSndInfo += str + Environment.NewLine;
                    }
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 13)
                {
                    int x = info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CommunicationChannel;
                    var changed = "";
                    for (int i = 1; i < 17; i++)
                    {
                        if ((x >> (i - 1) & 1) == 1) changed += i + "-";
                    }


                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  召测成功" + Environment.NewLine;
                    ShowSndInfo += "通信异常不成功次数：" + info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CommunicationFailures +
                                   Environment.NewLine;
                    // lvf 2018年4月8日13:46:04  功率因素 除以100
                    ShowSndInfo += "功率因数：" + Convert.ToDouble(info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.PowerFactor)/100 + Environment.NewLine;
                    ShowSndInfo += "通信信道：" + changed + Environment.NewLine;
                    ShowSndInfo += "电流最大量程：" + info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CurrentRange +
                                   Environment.NewLine;
                    ShowSndInfo += "有功最大量程：" + info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.PowerRange + Environment.NewLine;
                    ShowSndInfo += "集中器自动控制功能：" +
                                   (info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.AutoMode == 1 ? "自动补发" : "不补发") +
                                   Environment.NewLine;
                    ShowSndInfo += "经纬度：" + info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.Longitude + " - " +
                                   info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.Latitude +
                                   Environment.NewLine;
                    ShowSndInfo += "载波路由模式：" +
                                   (info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CarrierRoutingMode == 1
                                        ? "标准"
                                        : info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CarrierRoutingMode == 2
                                              ? "扩展"
                                              : info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CarrierRoutingMode == 3
                                                    ? "III代"
                                                    : info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.CarrierRoutingMode == 4
                                                          ? "IV代"
                                                          : "自适应") +
                                   Environment.NewLine;
                    ShowSndInfo += "蓝牙Pin码：" + info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.BluetoothPin +
                                   Environment.NewLine;
                    ShowSndInfo += "蓝牙安全模式：" +
                                   (info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.BluetoothMode == 0
                                        ? "无"
                                        : info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.BluetoothMode == 1 ? "安全模式1" : "安全模式2") +
                                   Environment.NewLine;
                    ShowSndInfo += "集中器通信类型：" + (info.WstSluSvrAnsZcOrSet.ZcJzqAlarmPara.Cct == 0 ? "Gprs" : "485") +
                                   Environment.NewLine;
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 14)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  召测成功" + Environment.NewLine;
                    ShowSndInfo += "软件版本：" + info.WstSluSvrAnsZcOrSet.ZcSoftVersion + Environment.NewLine;
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 15)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  召测成功" + Environment.NewLine;
                    ShowSndInfo += "时钟：" +
                                   new DateTime(info.WstSluSvrAnsZcOrSet.ZcJzqTime.DateTime).ToString(
                                       "yyyy-MM-dd HH:mm:ss") +
                                   Environment.NewLine;
                    //  ShowSndInfo += "强制对时：" + info.Data.ZcJzqTime.ForceTimer + Environment.NewLine;
                    ShowSndInfo += "时间数据错误：" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.DtformatError ? "是" : "否") +
                                   Environment.NewLine;
                    //  ShowSndInfo += "对时失败：" + (info.Data.ZcJzqTime.TimeFault ? "是" : "否") + Environment.NewLine;
                    ShowSndInfo += "时钟超差：" + (info.WstSluSvrAnsZcOrSet.ZcJzqTime.TimerError ? "是" : "否") +
                                   Environment.NewLine;
                }
                if (info.WstSluSvrAnsZcOrSet.Op == 16)
                {
                    ShowSndInfo += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收：" +
                                   StrNt[info.WstSluSvrAnsZcOrSet.Op - 1] +
                                   "  成功" + Environment.NewLine;

                    ShowSndInfo += "复位集中器：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.ReConcentrator ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "外部复位Zigbee模块：" +
                                   (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.HardReZigbee ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "软件复位Zigbee模块：" +
                                   (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.SoftReZigbee ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "复位电力载波模块：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.ReCarrier ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "初始化所有硬件配置：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.InitAll ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "初始化数据区：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.ClearData ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "初始化参数区：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.ClearArgs ? "是" : "否") +
                                   Environment.NewLine;
                    ShowSndInfo += "清除缓存未执行任务：" + (info.WstSluSvrAnsZcOrSet.ZcSetResetAndInit.ClearTask ? "是" : "否") +
                                   Environment.NewLine;
                }
                //private static  string[] StrNt = new string[16]
                //                             {
                //                                 "复位网络1", "复位网络2", "复位网络3", "复位网络4", "设置集中器巡测", "设置集中器报警和投运停运", "设置集中器参数",  1-标准，2-扩展，3-III代，4-IV代，5-自适应
                //                                 "设置集中器报警参数", "设置控制器域名参数", "设置时钟",
                //                                 "11召测集中器参数", "召测控制器域名修改信息", "召测集中器报警参数", "召测软件版本", "召测时钟", "复位及网络初始化"

                //                             };
            }

        

        }

        /// <summary>
        /// 单灯参数分组信息
        /// </summary>
        public partial class ConcentratorParaInformationViewModel
        {
            private void InitGroupViewModel()
            {
                grpFlag = false;
                StartNode = 1;
                NodeSpace = 1;
                TreeItems.Clear();
                foreach (var t in ControlParaItems)
                {
                    if (t.IsChecked)
                        t.IsChecked = false;
                }
                //对分组子节点 进行数据加载
                //var grps = SrInfo.CtrGrpInfo.MySelf.GetGrpInfoBySluId(SingleId);

                foreach (var t in _terminalInformation.WjSluCtrlGrps.Values)
                {
                    this.TreeItems.Add(new TreeItemGrplViewModel(t, _terminalInformation.RtuId, true));
                    //t.AddChild(SingleId);
                }
                foreach (var tt in TreeItems)
                {
                    tt.AddChild();
                }
            }

            private List<Wlst.client.SluRegulatorGroupParameter> BackGroupViewModelSluCtrlgrp()
            {
                bool noCtrls = false;
                var updateLst = new List<Wlst.client.SluRegulatorGroupParameter>();
                foreach (var t in TreeItems)
                {
                    var grpControlLst = new List<int>();
                    foreach (var tt in t.ChildTreeItems)
                    {
                        if (tt.NodeId <= ControlParaItems.Count)
                            grpControlLst.Add(tt.NodeId);

                    }
                    var grpInfo = new Wlst.client.SluRegulatorGroupParameter()
                                      {

                                          GrpId = t.NodeId,
                                          GrpName = t.NodeName,
                                          CtrlPhyLst = grpControlLst
                                      };
                    if (grpControlLst.Count > 0)
                    {
                        updateLst.Add(grpInfo);
                    }
                    else
                    {
                        noCtrls = true;

                    }
                }
                if (noCtrls)
                {
                    var infos = WlstMessageBox.Show("提示", "分组中没有控制器，不能保存分组信息", WlstMessageBoxType.Close);
                }
  
                return updateLst;
                //_terminalInformation.WjSluCtrlGrps
                //SrInfo.CtrGrpInfo.MySelf.UpdateGroupInfo(updateLst, SingleId);
            }

            private int _startNode;

            /// <summary>
            /// 开始节点
            /// </summary>
            public int StartNode
            {
                get { return _startNode; }
                set
                {
                    if (_startNode != value)
                    {
                        _startNode = value;
                        this.RaisePropertyChanged(() => this.StartNode);
                    }
                }
            }

            private int _nodeSpace;

            /// <summary>
            /// 节点间隔
            /// </summary>
            public int NodeSpace
            {
                get { return _nodeSpace; }
                set
                {
                    if (_nodeSpace != value)
                    {
                        _nodeSpace = value;
                        this.RaisePropertyChanged(() => this.NodeSpace);
                    }
                }
            }

            private ObservableCollection<TreeItemGrplViewModel> _treeItems;

            /// <summary>
            /// 终端树  根节点
            /// </summary>
            public ObservableCollection<TreeItemGrplViewModel> TreeItems
            {
                get
                {
                    if (_treeItems == null)
                        _treeItems = new ObservableCollection<TreeItemGrplViewModel>();
                    return _treeItems;
                }
            }

            public void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {
                // 如果使用附加属性来开启右键选中功能，
                // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
            }

            public void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {
                // 注意，这里的sender是TreeView
                // 我们需要从e.OriginalSource拿到TreeViewItem
                var item = VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }


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
                    lsttitle.Add("条形码");
                    lsttitle.Add("灯杆编码");
                    lsttitle.Add("主动告警");
                    lsttitle.Add("投运");
                    lsttitle.Add("回路数量");
                    lsttitle.Add("回路1上电开灯");
                    lsttitle.Add("回路2上电开灯");
                    lsttitle.Add("回路3上电开灯");
                    lsttitle.Add("回路4上电开灯");
                    lsttitle.Add("回路1矢量");
                    lsttitle.Add("回路2矢量");
                    lsttitle.Add("回路3矢量");
                    lsttitle.Add("回路4矢量");
                    lsttitle.Add("回路1额定功率");
                    lsttitle.Add("回路2额定功率");
                    lsttitle.Add("回路3额定功率");
                    lsttitle.Add("回路4额定功率");
                    lsttitle.Add("功率上限(%)");
                    lsttitle.Add("功率下限(%)");
                    lsttitle.Add("开灯序号");



                    var lstobj = new List<List<object>>();

                    foreach (var g in ControlParaItems)
                    {
                        var tmp = new List<object>();
                        tmp.Add(g.Index);
                        tmp.Add(g.BarCode.ToString().PadLeft(16, '0'));
                        tmp.Add(g.LampCode);

                        var flg = "";
                        if (g.IsActiveAlarm) flg = "是";
                        else flg = "否";
                        tmp.Add(flg);

                        flg = "";
                        if (g.IsRun) flg = "是";
                        else flg = "否";
                        tmp.Add(flg);

                        tmp.Add(g.CurrentSelectLoopNumItem.Value);

                        if (g.CurrentSelectLoopNumItem.Value > 0)
                        {
                            flg = "";
                            if (g.IsEnableByLoop[0].IsSelected) flg = "是";
                            else flg = "否";
                        }
                        else
                        {
                            flg = "--";
                        }
                        tmp.Add(flg);

                        if (g.CurrentSelectLoopNumItem.Value > 1)
                        {
                            flg = "";
                            if (g.IsEnableByLoop[1].IsSelected) flg = "是";
                            else flg = "否";
                        }
                        else
                        {
                            flg = "--";
                        }
                        tmp.Add(flg);

                        if (g.CurrentSelectLoopNumItem.Value > 2)
                        {
                            flg = "";
                            if (g.IsEnableByLoop[2].IsSelected) flg = "是";
                            else flg = "否";
                        }
                        else
                        {
                            flg = "--";
                        }
                        tmp.Add(flg);

                        if (g.CurrentSelectLoopNumItem.Value > 3)
                        {
                            flg = "";
                            if (g.IsEnableByLoop[3].IsSelected) flg = "是";
                            else flg = "否";
                        }
                        else
                        {
                            flg = "--";
                        }
                        tmp.Add(flg);

                        if (g.CurrentSelectLoopNumItem.Value > 0)
                        {
                            tmp.Add(g.CurrentSelectLoopVectorItem1.Value);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 1)
                        {
                            tmp.Add(g.CurrentSelectLoopVectorItem2.Value);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 2)
                        {
                            tmp.Add(g.CurrentSelectLoopVectorItem3.Value);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 3)
                        {
                            tmp.Add(g.CurrentSelectLoopVectorItem4.Value);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 0)
                        {
                            tmp.Add(g.CurrentSelectLoopRatePowerIndex1.Name);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 1)
                        {
                            tmp.Add(g.CurrentSelectLoopRatePowerIndex2.Name);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 2)
                        {
                            tmp.Add(g.CurrentSelectLoopRatePowerIndex3.Name);
                        }
                        else
                            tmp.Add("--");

                        if (g.CurrentSelectLoopNumItem.Value > 3)
                        {
                            tmp.Add(g.CurrentSelectLoopRatePowerIndex4.Name);
                        }
                        else
                            tmp.Add("--");

                        tmp.Add(g.PowerMax);
                        tmp.Add(g.PowerMin);
                        tmp.Add(g.LightIndex);

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
                if (ControlParaItems.Count < 1) return false;
                return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
                return false;
            }

            #endregion

            #region CmdAddGroup

            private ICommand _cmdAddGroup;

            public ICommand CmdAddGroup
            {
                get { return _cmdAddGroup ?? (_cmdAddGroup = new RelayCommand(ExAddGroup, CanAddGroup, false)); }
            }

            private void ExAddGroup()
            {
                int childId = GetMaxGrpId();
                if (childId > 255) return;

                if (childId == -1) return;
                var gi = new Wlst.client.SluRegulatorGroupParameter()
                             {
                                 GrpId = childId,
                                 GrpName = "新控制器分组",
                             };
                this.TreeItems.Add(new TreeItemGrplViewModel(gi, _terminalInformation.RtuId, true));
            }

            private bool CanAddGroup()
            {
                return true;
            }

            public int GetMaxGrpId()
            {
                int max = 0;
                foreach (var t in this.TreeItems)
                {
                    if (t.NodeType == TreeNodeType.IsGrp)
                    {
                        if (t.NodeId > max) max = t.NodeId;
                    }
                }
                return max + 1;
            }

            #endregion

            #region CmdCancelGroup

            private ICommand _cmdCancelGroup;

            public ICommand CmdCancelGroup
            {
                get { return _cmdCancelGroup ?? (_cmdCancelGroup = new RelayCommand(ExCancelGroup, CanCancelGroup, false)); }
            }

            private void ExCancelGroup()
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要删除选中控制器分组，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }

                for (int i = 0; i < TreeItems.Count; i++)
                {
                    if (TreeItems[i].IsSelected)
                    {
                        if (TreeItems[i].NodeType == TreeNodeType.IsGrp)
                        {
                            TreeItems.RemoveAt(i);
                        }
                    }
                }
            }

            private bool CanCancelGroup()
            {
                return true;
            }

            #endregion

            #region CmdAddToGrp

            private ICommand _cmdAddToGrp;

            public ICommand CmdAddToGrp
            {
                get { return _cmdAddToGrp ?? (_cmdAddToGrp = new RelayCommand(ExAddToGrp, CanAddToGrp, false)); }
            }

            private void ExAddToGrp()
            {
                foreach (var t in TreeItems)
                {
                    if (t.IsTreeChecked)
                    {
                        foreach (var tt in ControlParaItems)
                        {
                            if (tt.IsChecked)
                            {
                                bool find = false;
                                for (int i = 0; i < t.ChildTreeItems.Count; i++)
                                {
                                    if (t.ChildTreeItems[i].NodeId == tt.Index)
                                    {
                                        find = true;
                                    }
                                }
                                if (find) continue;

                                var gi = new SluRegulatorParameter()
                                             {
                                                 CtrlId = tt.Index,
                                                 RtuName = "" + tt.LampCode,
                                                 LampCode = tt.LampCode,
                                                 CtrlPhyId = tt.Index
                                             };
                                t.ChildTreeItems.Add(new TreeItemGrplViewModel(gi, _terminalInformation.RtuId, false));
                                ////tt.IsChecked = false;
                            }
                        }
                    }
                }
            }

            private bool CanAddToGrp()
            {
                bool flag = false;
                foreach (var t in ControlParaItems)
                {
                    if (t.IsChecked)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }

            #endregion

            #region CmdCancelFromGrp

            private ICommand _cmdCancelFromGrp;

            public ICommand CmdCancelFromGrp
            {
                get
                {
                    return _cmdCancelFromGrp ??
                           (_cmdCancelFromGrp = new RelayCommand(ExCancelFromGrp, CanCancelFromGrp, false));
                }
            }

            private void ExCancelFromGrp()
            {
                for (int i = TreeItems.Count - 1; i >= 0; i--)
                {
                    if (TreeItems[i].IsTreeChecked)
                    {
                        //for (int j = TreeItems[i].ChildTreeItems.Count - 1; j >= 0;j-- )
                        //{
                        //    TreeItems[i].ChildTreeItems.RemoveAt(j);
                        //}
                        //TreeItems[i].IsTreeChecked = false;
                        TreeItems.RemoveAt(i);
                    }
                    else
                    {
                        for (int j = TreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (TreeItems[i].ChildTreeItems[j].IsTreeChecked)
                            {
                                TreeItems[i].ChildTreeItems.RemoveAt(j);
                                //  TreeItems[i].ChildTreeItems[j].IsTreeChecked = false;
                                //  break;
                            }
                        }
                    }
                }
            }

            private bool CanCancelFromGrp()
            {
                bool flag = false;
                foreach (var t in TreeItems)
                {
                    if (t.IsTreeChecked)
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        foreach (var tt in t.ChildTreeItems)
                        {
                            if (tt.IsTreeChecked)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                return flag;
            }

            #endregion

            #region CmdSglNode

            private ICommand _cmdSglNode;

            public ICommand CmdSglNode
            {
                get { return _cmdSglNode ?? (_cmdSglNode = new RelayCommand(ExSglNode, CanSglNode, false)); }
            }

            private void ExSglNode()
            {
                foreach (var t in ControlParaItems)
                {
                    t.IsChecked = false;
                }
                for (int i = 0; i < ControlParaItems.Count; i += 2)
                {
                    ControlParaItems[i].IsChecked = true;
                }
            }

            private bool CanSglNode()
            {
                return true;
            }

            #endregion

            #region CmdDblNode

            private ICommand _cmdDblNode;

            public ICommand CmdDblNode
            {
                get { return _cmdDblNode ?? (_cmdDblNode = new RelayCommand(ExDblNode, CanDblNode, false)); }
            }

            private void ExDblNode()
            {
                foreach (var t in ControlParaItems)
                {
                    t.IsChecked = false;
                }
                for (int i = 1; i < ControlParaItems.Count; i += 2)
                {
                    ControlParaItems[i].IsChecked = true;
                }
            }

            private bool CanDblNode()
            {
                return true;
            }

            #endregion

            #region CmdCleanSelected

            private ICommand _cmdCleanSelected;

            public ICommand CmdCleanSelected
            {
                get
                {
                    return _cmdCleanSelected ??
                           (_cmdCleanSelected = new RelayCommand(ExCleanSelected, CanCleanSelected, false));
                }
            }

            private void ExCleanSelected()
            {
                foreach (var tt in ControlParaItems)
                {
                    if (tt.IsChecked)
                    {
                        tt.IsChecked = false;
                    }
                }
            }

            private bool CanCleanSelected()
            {
                return true;
            }

            #endregion

            #region CmdSelectAll

            private ICommand _cmdSelectAll;

            public ICommand CmdSelectAll
            {
                get { return _cmdSelectAll ?? (_cmdSelectAll = new RelayCommand(ExSelectAll, CanSelectAll, false)); }
            }

            private void ExSelectAll()
            {
                foreach (var tt in ControlParaItems)
                {
                    tt.IsChecked = true;
                }
            }

            private bool CanSelectAll()
            {
                return true;
            }

            #endregion

            #region CmdSelected

            private ICommand _cmdSelected;

            public ICommand CmdSelected
            {
                get { return _cmdSelected ?? (_cmdSelected = new RelayCommand(ExSelected, CanSelected, false)); }
            }

            private void ExSelected()
            {
                foreach (var t in ControlParaItems)
                {
                    t.IsChecked = false;
                }
                for (int i = StartNode - 1; i < ControlParaItems.Count; i += NodeSpace + 1)
                {
                    ControlParaItems[i].IsChecked = true;
                }
            }

            private bool CanSelected()
            {
                return true;
            }

            #endregion

            //#region CmdZcDelay
            //private ICommand _cmdZcDelay;

            //public ICommand CmdZcDelay
            //{
            //    get { return _cmdZcDelay ?? (_cmdZcDelay = new RelayCommand(ExAddsdfsdfGroup, CanAsdfsdfddGroup, false)); }
            //}

            //private void ExAddsdfsdfGroup()
            //{
            //    this.ZcDelayInfo();
            //}

            //private bool CanAsdfsdfddGroup()
            //{

            //    return this.IsZigbee == false;
            //}


            //#endregion
        }
    
}
