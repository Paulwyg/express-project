using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj2090Module.NewData.Services;

namespace Wlst.Ux.Wj2090Module.NewData.ViewModel
{

    [Export(typeof (IINewData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
                                            IINewData,Wlst .Cr .CoreMims .CoreInterface .IIShowData 
    {
        public void NavOnLoad(params object[] parsObjects)
        {
            VisiArgs = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 2, false)
                         ? Visibility.Visible
                         : Visibility.Collapsed;
        }

        public NewDataViewModel()
        {
            // this.AddEventFilterInfo(Wj2090Module.Services.EventIdAssign.OnSluNewDataArrive, PublishEventType.Core);
            this.AddEventFilterInfo( Wlst .Sr .EquipmentInfoHolding .Services.EventIdAssign.RunningInfoUpdate2 , PublishEventType.Core);
            this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);

            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab,
                PublishEventType.Core);

          
        }

        public override bool FundOrderFilterForExtendCheck(PublishEventArgs args)
        {
            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
            {
                if (args.GetParams().Count == 0) return false;
                var rtuids = args.GetParams()[0] as List<int>;
                if (rtuids == null || rtuids.Count == 0) return false;
                //if (rtuids.Contains(Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId) == false)
                //    return false;
                if (rtuids.Contains(CurrentSelectedRtuId)) // todo 20171127
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        protected int CurrentSelectedRtuId = 0;
        protected int CurrentSelectedCtrlId = 0;
        protected int CurrentApplicationSelectd = 0;
        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            try
            {

                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                {
                    if (args.GetParams().Count == 0) return;
                    var rtuids = args.GetParams()[0] as List<int>;
                    if (rtuids == null || rtuids.Count == 0) return;

                    if (args.GetParams().Count < 2)
                    {
                        if (rtuids.Contains(CurrentSelectedRtuId))
                        {
                            //OnSluDataArrive(lst);
                            OnSelectRtu(CurrentSelectedRtuId, 0);
                        }
                        return;
                    }

                    var lst = args.GetParams()[1] as List<int>;
                    if (rtuids.Contains(CurrentSelectedRtuId)) // todo 20171127
                    {
                        //OnSluDataArrive(lst);
                        OnSluDataArrive(new List<int>(){5});
                    }
                }
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int rtuid = Convert.ToInt32(args.GetParams()[0]);
                    CurrentApplicationSelectd = rtuid;
                   // CurrentSelectedCtrlId = 0;
                    if (rtuid > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart &&
                        rtuid < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd)
                    {
                        // OnSluDataUpdate();
                        if (args.GetParams().Count > 1)
                        {
                            int ctrlid = Convert.ToInt32(args.GetParams()[1]);
                            if (ctrlid < 1) ctrlid = 0;
                            OnSelectRtu(rtuid, ctrlid);

                           // DateTimeCtrl = DateTime.Now.Ticks+"";
                        }
                        else
                        {
                            OnSelectRtu(rtuid, 0);
                        }

                    }
                }
                if(args .EventId ==Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab)
                {
                    var info = args.GetParams()[0] as Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData;
                    if (info == null) return;
                    OnOtherViewShowData(info);
                    SelectedViewId = 5;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Slu New Data处理出错:" + ex);
            }
        }


        void OnOtherViewShowData(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData data)
        {
            try
            {
                if (data == null) return;

               UpdateCtrlData5(data );
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
               Wj2090Module.Services.ViewIdAssign.NewDataViewId);
            }
            catch (Exception ex)
            {

            }
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "最新数据"; }
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


        private int  _lDatsdfsdfeCreate;

        public int  SelectedViewId
        {
            get { return _lDatsdfsdfeCreate; }
            set
            {
                if (_lDatsdfsdfeCreate == value) return;
                _lDatsdfsdfeCreate = value;
                RaisePropertyChanged(() => SelectedViewId);
            }
        }
   

        public static int GetPhyIdByRtuId(int sluId,int ctrId)
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (info == null) return 0;
            var tmps = info as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (tmps == null) return 0;
            if (tmps.WjSluCtrls   .ContainsKey( ctrId ) )
            {
                return tmps.WjSluCtrls [ctrId].CtrlPhyId;
            }
            return 0;
        }
        public static string GetLampCode(int sluid, int ctrId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey(sluid))
                return "";
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid]
                as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

            if (t == null)
                return "";
            if (t.WjSluCtrls.ContainsKey(ctrId))
            {
                return t.WjSluCtrls[ctrId].LampCode;
            }
            return "";

        }

        public static string GetBarCode(int sluid, int ctrId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey(sluid))
                return "";
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid]
                as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

            if (t == null)
                return "";
            if (t.WjSluCtrls.ContainsKey(ctrId))
            {
                return t.WjSluCtrls[ctrId].BarCodeId +"";
            }
            return "";

        }

        private Visibility timeInfovis;
        public Visibility VisiTimeInfo
        {
            get { return timeInfovis; }
            set
            {
                if (value == timeInfovis) return;
                timeInfovis = value;
                this .RaisePropertyChanged(()=>this .VisiTimeInfo );
            }
        }


        private Visibility timeInfovidatas;
        public Visibility VisiData
        {
            get { return timeInfovidatas; }
            set
            {
                if (value == timeInfovidatas) return;
                timeInfovidatas = value;
                this.RaisePropertyChanged(() => this.VisiData);
            }
        }
    }

    /// <summary>
    /// Slu Attri  Data 1
    /// </summary>
    public partial class NewDataViewModel
    {
        private ObservableCollection<Wlst.Cr.CoreOne.Models.ObservableObjectCollection<string>> timeInfo;
        public ObservableCollection<Wlst.Cr.CoreOne.Models.ObservableObjectCollection<string>> TimeInfo
        {
            get
            {
                if (timeInfo == null) timeInfo = new ObservableCollection<ObservableObjectCollection<string>>();
                return timeInfo;
            }
        }

        void SetSluNameId(int sluId)
        {
            this.RtuId = sluId;
            var infs = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (infs != null)
            {
                RtuName = infs.RtuName ;
                RtuPhyId = infs.RtuPhyId .ToString("D4");
            }
            else
            {
                RtuName = "----";
                RtuPhyId = sluId.ToString("D4");
            }

        }
       

        void UpdateSluData(int sluId)
        {
            SetSluNameId(sluId);

           
            var ntgs = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeToday(sluId);
            var ntgg = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeDetailToday(ntgs, sluId, false);
            TimeInfo.Clear();
            foreach (var g in ntgg )
            {
                TimeInfo.Add(new ObservableObjectCollection<string>(4)
                                 {Value0 = g.Item1, Value1 = g.Item2, Value2 = g.Item3, Value3 = g.Item4});
            }


            VisiTimeInfo = ntgg.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if (runninfo == null || runninfo.SluNewData  == null||runninfo .SluNewData .SluData ==null )
            {
                ResetAllAttri1();
    
                return;
            }

            var tmp = runninfo.SluNewData.SluData ;
   
            DateCreate = new DateTime(tmp.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            Rest0 = tmp.Rest0;
            Rest1 = tmp.Rest1;
            Rest2 = tmp.Rest2;
            Rest3 = tmp.Rest3;
            IsSluStop = tmp.IsSluStop ? "是" : "否";
            IsEnableAlarm = tmp.IsEnableAlarm ? "是" : "否";
            IsPowerOn = tmp.IsPowerOn ? "是" : "否";
            IsGprs = tmp.IsGprs ? "是" : "否";
            IsConcentratorArgsError = tmp.IsConcentratorArgsError ? "是" : "否";
            IsCtrlArgsError = tmp.IsCtrlArgsError ? "是" : "否";
            IsZigbeeError = tmp.IsZigbeeError ? "是" : "否";
            IsCarrierError = tmp.IsCarrierError ? "是" : "否";
            IsFramError = tmp.IsFramError ? "是" : "否";
            IsBluetoothError = tmp.IsBluetoothError ? "是" : "否";
            IsTimerError = tmp.IsTimerError ? "是" : "否";
            UnknowCtrlCount = tmp.UnknowCtrlCount+"";
            CommunicationChannel = tmp.CommunicationChannel+"";
        }


        void ResetAllAttri1()
        {
            DateCreate = "----";
            Rest0 = 0;
            Rest1 = 0;
            Rest2 = 0;
            Rest3 = 0;
            IsSluStop = "--";
            IsEnableAlarm = "--";
            IsPowerOn = "--";
            IsGprs = "--";
            IsConcentratorArgsError = "--";
            IsCtrlArgsError = "--";
            IsZigbeeError = "--";
            IsCarrierError = "--";
            IsFramError = "--";
            IsBluetoothError = "--";
            IsTimerError = "--";
            UnknowCtrlCount = "--";
            CommunicationChannel = "--";
        }

        /// <summary>
        /// 数据发生时间  与回路数据联合查询组合成最新数据
        /// </summary>

        private string _lDateCreate;

        public string  DateCreate
        {
            get { return _lDateCreate; }
            set
            {
                if (_lDateCreate == value) return;
                _lDateCreate = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }
        private int _lRtuId;
        public int RtuId
        {
            get { return _lRtuId; }
            set
            {
                if (_lRtuId == value) return;
                _lRtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        private string  _lsdfsdId;
        public string  RtuPhyId
        {
            get { return _lsdfsdId; }
            set
            {
                if (_lsdfsdId == value) return;
                _lsdfsdId = value;
                RaisePropertyChanged(() => RtuPhyId);
            }
        }
        private string  _lRRtuName;
        public string  RtuName
        {
            get { return _lRRtuName; }
            set
            {
                if (_lRRtuName == value) return;
                _lRRtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }
        private int _lRest3;
        /// <summary>
        /// 复位次数
        /// </summary>

        public int Rest3
        {
            get { return _lRest3; }
            set
            {
                if (_lRest3 == value) return;
                _lRest3 = value;
                RaisePropertyChanged(() => Rest3);
            }
        }
        private int _lRest2;
        /// <summary>
        /// 复位次数
        /// </summary>

        public int Rest2
        {
            get { return _lRest2; }
            set
            {
                if (_lRest2 == value) return;
                _lRest2 = value;
                RaisePropertyChanged(() => Rest2);
            }
        }
        private int _lRest1;
        /// <summary>
        /// 复位次数
        /// </summary>

        public int Rest1
        {
            get { return _lRest1; }
            set
            {
                if (_lRest1 == value) return;
                _lRest1 = value;
                RaisePropertyChanged(() => Rest1);
            }
        }
        private int _lRest0;
        /// <summary>
        /// 复位次数
        /// </summary>

        public int Rest0
        {
            get { return _lRest0; }
            set
            {
                if (_lRest0 == value) return;
                _lRest0 = value;
                RaisePropertyChanged(() => Rest0);
            }
        }
        private string _lIsSluStop;
        /// <summary>
        /// 停运 0-正常，1-停运
        /// </summary>

        public string IsSluStop
        {
            get { return _lIsSluStop; }
            set
            {
                if (_lIsSluStop == value) return;
                _lIsSluStop = value;
                RaisePropertyChanged(() => IsSluStop);
            }
        }
        private string _lIsEnableAlarm;
        /// <summary>
        /// 允许主报 0-禁止主报，1-允许主报
        /// </summary>

        public string IsEnableAlarm
        {
            get { return _lIsEnableAlarm; }
            set
            {
                if (_lIsEnableAlarm == value) return;
                _lIsEnableAlarm = value;
                RaisePropertyChanged(() => IsEnableAlarm);
            }
        }
        private string _lIsPowerOn;
        /// <summary>
        /// 开机申请 0-非开机，1-开机
        /// </summary>

        public string IsPowerOn
        {
            get { return _lIsPowerOn; }
            set
            {
                if (_lIsPowerOn == value) return;
                _lIsPowerOn = value;
                RaisePropertyChanged(() => IsPowerOn);
            }
        }
        private string _lIsGprs;
        /// <summary>
        /// gprs通讯 0-485,1-gprs
        /// </summary>

        public string IsGprs
        {
            get { return _lIsGprs; }
            set
            {
                if (_lIsGprs == value) return;
                _lIsGprs = value;
                RaisePropertyChanged(() => IsGprs);
            }
        }
        private string _lIsConcentratorArgsError;
        /// <summary>
        /// 集中器参数错误
        /// </summary>

        public string IsConcentratorArgsError
        {
            get { return _lIsConcentratorArgsError; }
            set
            {
                if (_lIsConcentratorArgsError == value) return;
                _lIsConcentratorArgsError = value;
                RaisePropertyChanged(() => IsConcentratorArgsError);
            }
        }
        private string _lIsCtrlArgsError;
        /// <summary>
        /// 控制器参数错误
        /// </summary>

        public string IsCtrlArgsError
        {
            get { return _lIsCtrlArgsError; }
            set
            {
                if (_lIsCtrlArgsError == value) return;
                _lIsCtrlArgsError = value;
                RaisePropertyChanged(() => IsCtrlArgsError);
            }
        }
        private string _lIsZigbeeError;

        /// <summary>
        /// zigbee模块出错
        /// </summary>

        public string IsZigbeeError
        {
            get { return _lIsZigbeeError; }
            set
            {
                if (_lIsZigbeeError == value) return;
                _lIsZigbeeError = value;
                RaisePropertyChanged(() => IsZigbeeError);
            }
        }
        private string _lIsCarrierError;
        /// <summary>
        /// 电力载波模块出错
        /// </summary>

        public string IsCarrierError
        {
            get { return _lIsCarrierError; }
            set
            {
                if (_lIsCarrierError == value) return;
                _lIsCarrierError = value;
                RaisePropertyChanged(() => IsCarrierError);
            }
        }
        private string _lIsFramError;
        /// <summary>
        /// fram出错
        /// </summary>

        public string IsFramError
        {
            get { return _lIsFramError; }
            set
            {
                if (_lIsFramError == value) return;
                _lIsFramError = value;
                RaisePropertyChanged(() => IsFramError);
            }
        }
        private string _lIsBluetoothError;
        /// <summary>
        /// 蓝牙模块出错
        /// </summary>

        public string IsBluetoothError
        {
            get { return _lIsBluetoothError; }
            set
            {
                if (_lIsBluetoothError == value) return;
                _lIsBluetoothError = value;
                RaisePropertyChanged(() => IsBluetoothError);
            }
        }
        private string _lIsTimerError;
        /// <summary>
        /// 硬件时钟出错
        /// </summary>

        public string IsTimerError
        {
            get { return _lIsTimerError; }
            set
            {
                if (_lIsTimerError == value) return;
                _lIsTimerError = value;
                RaisePropertyChanged(() => IsTimerError);
            }
        }

        private string  _lUnknowCtrlCount;
        /// <summary>
        /// 未知控制器
        /// </summary>

        public string  UnknowCtrlCount
        {
            get { return _lUnknowCtrlCount; }
            set
            {
                if (_lUnknowCtrlCount == value) return;
                _lUnknowCtrlCount = value;
                RaisePropertyChanged(() => UnknowCtrlCount);
            }
        }
        private string _lCommunicationChannel;
        /// <summary>
        /// 通信信道 -10为当前值
        /// </summary>

        public string  CommunicationChannel
        {
            get { return _lCommunicationChannel; }
            set
            {
                if (_lCommunicationChannel == value) return;
                _lCommunicationChannel = value;
                RaisePropertyChanged(() => CommunicationChannel);
            }
        }


        private Visibility _visiArgs;
        /// <summary>
        /// 参数是否显示
        /// </summary>
        public Visibility VisiArgs
        {
            get { return _visiArgs; }
            set
            {
                if (_visiArgs == value) return;
                _visiArgs = value;
                RaisePropertyChanged(() => VisiArgs);
            }
        }

        private string _barCode;
        /// <summary>
        /// 控制器条形码
        /// </summary>
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                if (_barCode == value) return;
                _barCode = value;
                RaisePropertyChanged(() => BarCode);
            }
        }

    }

    /// <summary>
    /// Slu Attri  Unkown 2
    /// </summary>
    public partial class NewDataViewModel
    {

        private ObservableCollection<UnknowCtrlVm> _isPowerOnLight;

        public ObservableCollection<UnknowCtrlVm> Items
        {
            get
            {
                if (_isPowerOnLight == null)
                {
                    _isPowerOnLight = new ObservableCollection<UnknowCtrlVm>();

                }
                return _isPowerOnLight;
            }
            set
            {
                if (_isPowerOnLight == value) return;
                _isPowerOnLight = value;
                RaisePropertyChanged(() => Items);
            }
        }


        private void UpdateSluDataUnkown2(int sluId)
        {
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if (runninfo == null || runninfo.SluNewData == null||runninfo .SluNewData .DataUnknown ==null ) return;
            Items.Clear();
  

            var lst = new ObservableCollection<UnknowCtrlVm>();
            foreach (var g in runninfo.SluNewData.DataUnknown)
            {
                lst.Add(new UnknowCtrlVm(g));
            }
            this.Items = lst;
        }
    }
    /// <summary>
    /// Ctrl Attri Phy 4
    /// </summary>
    public partial class NewDataViewModel
    {
        void UpdateCtrlData4(int sluId, int ctrlId)
        {
            SetSluNameId(sluId); 
            this.CtrlId = ctrlId;
            this.CtrlPhyId = GetPhyIdByRtuId(sluId, ctrlId);
           // var tukey = new Tuple<int, int>(sluId, ctrlId);

            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if (runninfo == null || runninfo.SluCtrlNewData    == null  ) return;
            if (!runninfo.SluCtrlNewData.ContainsKey(ctrlId ))
            {
                ResetAllAttri4();
                return;
            }


            var tmp = runninfo.SluCtrlNewData[ctrlId ].DataPhy4;
            if (tmp == null)
            {
                ResetAllAttri4();
                return;
            }
            
           
            SignalStrength = tmp.SignalStrength;
            Phase = tmp.Phase == 1 ? "A相" : tmp.Phase == 2 ? "B相" : tmp.Phase == 3 ? "C相" : "未知";
            UsefulCommunicate = tmp.UsefulCommunicate;
            AllCommunicate = tmp.AllCommunicate;
            CtrlLoop = tmp.CtrlLoop;
            PowerSaving = tmp.PowerSaving == 0
                              ? "无控制"
                              : tmp.PowerSaving == 1
                                    ? "只有开关灯"
                                    : tmp.PowerSaving == 2
                                          ? "调档节能"
                                          : tmp.PowerSaving == 3
                                                ? "调光节能"
                                                : tmp.PowerSaving == 4 ? "RS485" : "调光";
            HasLeakage = tmp.HasLeakage ? "有" : "无";
            HasTemperature = tmp.HasTemperature ? "有" : "无";
            HasTimer = tmp.HasTimer ? "有" : "无";
            Model = tmp.Model == 1 ? "wj2090j" : "未知";

        }


        void ResetAllAttri4()
        {
            SignalStrength = 0;
            Phase = "未知";
            UsefulCommunicate = 0;
            AllCommunicate = 0;
            CtrlLoop = 0;
            PowerSaving = "--";
            HasLeakage =
                "--";
            HasTemperature = "--";
            HasTimer = "--";
            Model = "--";

        }

        #region attri



        private int _isdfsdfndexsdf;

        public int CtrlId
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        private int _issdfsddfsdfndexsdf;

        public int CtrlPhyId
        {
            get { return _issdfsddfsdfndexsdf; }
            set
            {
                if (_issdfsddfsdfndexsdf == value) return;
                _issdfsddfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlPhyId);
            }
        }


        /// <summary>
        /// 灯杆编码
        /// </summary>
        private string _ctrlLampCode;

        public string CtrlLampCode
        {
            get { return _ctrlLampCode; }
            set
            {
                if (_ctrlLampCode == value) return;
                _ctrlLampCode = value;
                RaisePropertyChanged(() => CtrlLampCode);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>

        #region SluId

        private int _indexsdf;

        public int SignalStrength
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => SignalStrength);
            }
        }

        private string _inPhasedexsdf;

        public string  Phase
        {
            get { return _inPhasedexsdf; }
            set
            {
                if (_inPhasedexsdf == value) return;
                _inPhasedexsdf = value;
                RaisePropertyChanged(() => Phase);
            }
        }
        private int _iUsefulCommunicatendexsdf;

        public int UsefulCommunicate
        {
            get { return _iUsefulCommunicatendexsdf; }
            set
            {
                if (_iUsefulCommunicatendexsdf == value) return;
                _iUsefulCommunicatendexsdf = value;
                RaisePropertyChanged(() => UsefulCommunicate);
            }
        }

        private int _indAllCommunicateexsdf;

        public int AllCommunicate
        {
            get { return _indAllCommunicateexsdf; }
            set
            {
                if (_indAllCommunicateexsdf == value) return;
                _indAllCommunicateexsdf = value;
                RaisePropertyChanged(() => AllCommunicate);
            }
        }    private int _indeCtrlLoopxsdf;

        public int CtrlLoop
        {
            get { return _indeCtrlLoopxsdf; }
            set
            {
                if (_indeCtrlLoopxsdf == value) return;
                _indeCtrlLoopxsdf = value;
                RaisePropertyChanged(() => CtrlLoop);
            }
        }




        private string _indsdfsdfdf;

        public string PowerSaving
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => PowerSaving);
            }
        }

        private string _index;

        public string HasLeakage
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => HasLeakage);
            }
        }

        #endregion


        private string _lHasTemperature;

        public string HasTemperature
        {
            get { return _lHasTemperature; }
            set
            {
                if (_lHasTemperature == value) return;
                _lHasTemperature = value;
                RaisePropertyChanged(() => HasTemperature);
            }
        }


        private string _lDateReply;

        public string HasTimer
        {
            get { return _lDateReply; }
            set
            {
                if (_lDateReply == value) return;
                _lDateReply = value;
                RaisePropertyChanged(() => HasTimer);
            }
        }


        private string _liUserName;

        public string Model
        {
            get { return _liUserName; }
            set
            {
                if (_liUserName == value) return;
                _liUserName = value;
                RaisePropertyChanged(() => Model);
            }
        }

        #endregion




    }

    /// <summary>
    /// Ctrl Attri Data 5 
    /// </summary>
    public partial class NewDataViewModel
    {
        private void UpdateCtrlData5(int sluId, int ctrlId)
        {
            SetSluNameId(sluId);
            this.CtrlId = ctrlId;
            this.CtrlPhyId = GetPhyIdByRtuId(sluId, ctrlId);
            this.CtrlLampCode = GetLampCode(sluId, ctrlId);
            this.BarCode = GetBarCode(sluId, ctrlId).PadLeft(13, '0');
            var tukey = new Tuple<int, int>(sluId, ctrlId);


            var ntgs = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeToday(sluId, CtrlPhyId);
            var ntgg = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeDetailToday(ntgs, sluId, true);
            TimeInfo.Clear();
            foreach (var g in ntgg)
            {
                TimeInfo.Add(new ObservableObjectCollection<string>(4) { Value0 = g.Item1, Value1 = g.Item2, Value2 = g.Item3, Value3 = "" });
            }
            VisiTimeInfo = ntgg.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if (runninfo == null || runninfo.SluCtrlNewData == null || runninfo.SluCtrlNewData .ContainsKey( ctrlId )==false )
            {
                VisiData = Visibility.Collapsed;
                ResetAllAttri5();
                return;
            }


            var tmp = runninfo.SluCtrlNewData[ctrlId ].Data5;
            if (tmp == null)
            {
                VisiData = Visibility.Collapsed;
                ResetAllAttri5();
                return;
            }

            DateCreate = new DateTime(tmp.Info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            OrderId = tmp.Info.OrderId;
            DateTimeCtrl = new DateTime(tmp.Info.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss");
            Temperature = tmp.Info.Temperature;
            Status = tmp.Info.Status == 0
                         ? "正常"
                         : tmp.Info.Status == 1
                               ? "电压越上限"
                               : tmp.Info.Status == 2
                                     ? "电压越下限"
                                     : "通讯故障";
            IsAdjust = tmp.Info.IsAdjust ? "已校准" : "未校准";
            IsWorkingArgsSet = tmp.Info.IsWorkingArgsSet ? "已设置" : "未设置";
            IsNoAlarm = tmp.Info.IsNoAlarm ? "禁止" : "允许";
            IsCtrlStop = tmp.Info.IsCtrlStop ? "停运" : "正常";
            IsEepromError = tmp.Info.IsEepromError ? "故障" : "正常";
            IsTemperatureSensor = tmp.Info.IsTemperatureSensor ? "故障" : "正常";

            DataSluCtrlLampItems.Clear();

            foreach (var t in tmp .Items )
            {
                this.DataSluCtrlLampItems.Add(new DataSluCtrlLampVm(t,tmp.Info .Status ));
            }
            VisiData = tmp.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }


        private void ResetAllAttri5()
        {
            DateCreate = "----";
            OrderId = 0;
            DateTimeCtrl = "----";
            Temperature = 0;
            Status = "--";
            IsAdjust = "--";
            IsWorkingArgsSet = "--";
            IsNoAlarm = "--";
            IsCtrlStop = "--";
            IsEepromError = "--";
            IsTemperatureSensor = "--";
            DataSluCtrlLampItems.Clear();
        }

        private void UpdateCtrlData5(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData data)
        {
            int sluId = data.Info.SluId;
            int ctrlData = data.Info.CtrlId;
            SetSluNameId(data .Info .SluId );
            this.CtrlId = data .Info .CtrlId ;
            this.CtrlPhyId = GetPhyIdByRtuId(data.Info.SluId, data.Info.CtrlId);
            this.CtrlLampCode = GetLampCode(sluId, data.Info.CtrlId);
            //var tukey = new Tuple<int, int>(sluId, data.Info.CtrlId);


            //var ntgs = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeToday(sluId, CtrlPhyId);
            //var ntgg = Wj2090Module.SrInfo.TimeInfos.MySelf.GetSluBandingSchemeDetailToday(ntgs, sluId, true);
            //TimeInfo.Clear();
            //foreach (var g in ntgg)
            //{
            //    TimeInfo.Add(new ObservableObjectCollection<string>(4) { Value0 = g.Item1, Value1 = g.Item2, Value2 = g.Item3, Value3 = "" });
            //}
            VisiTimeInfo = Visibility.Collapsed;





            var tmp = data;

            DateCreate = new DateTime(tmp.Info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            OrderId = tmp.Info.OrderId;
            if(tmp .Info .DateTimeCtrl <0)
            {
                DateTimeCtrl = "-- 终端报警数据 --" + "  -- 历史数据 --";
            }
           else  if (tmp.Info.DateTimeCtrl == 0)
            {
                DateTimeCtrl = "-- 通信故障 --" + "  -- 历史数据 --";
            }
            else 
            DateTimeCtrl = new DateTime(tmp.Info.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss")+"  -- 历史数据 --";
            Temperature = tmp.Info.Temperature;
            Status = tmp.Info.Status == 0
                         ? "正常"
                         : tmp.Info.Status == 1
                               ? "电压越上限"
                               : tmp.Info.Status == 2
                                     ? "电压越下限"
                                     : "通讯故障";
            IsAdjust = tmp.Info.IsAdjust ? "已校准" : "未校准";
            IsWorkingArgsSet = tmp.Info.IsWorkingArgsSet ? "已设置" : "未设置";
            IsNoAlarm = tmp.Info.IsNoAlarm ? "禁止" : "允许";
            IsCtrlStop = tmp.Info.IsCtrlStop ? "停运" : "正常";
            IsEepromError = tmp.Info.IsEepromError ? "故障" : "正常";
            IsTemperatureSensor = tmp.Info.IsTemperatureSensor ? "故障" : "正常";

            DataSluCtrlLampItems.Clear();

            foreach (var t in tmp.Items)
            {
                this.DataSluCtrlLampItems.Add(new DataSluCtrlLampVm(t, tmp.Info.Status));
            }
            VisiData = tmp.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        #region attri



        private ObservableCollection<DataSluCtrlLampVm> _iDataSluCtrlLamptems;

        public ObservableCollection<DataSluCtrlLampVm> DataSluCtrlLampItems
        {
            get
            {
                if (_iDataSluCtrlLamptems == null)
                {
                    _iDataSluCtrlLamptems = new ObservableCollection<DataSluCtrlLampVm>();

                }
                return _iDataSluCtrlLamptems;
            }
            set
            {
                if (_iDataSluCtrlLamptems == value) return;
                _iDataSluCtrlLamptems = value;
                RaisePropertyChanged(() => DataSluCtrlLampItems);
            }
        }



        public int SluIOrderIdd;

        /// <summary>
        /// 控制器地址 排序地址
        /// </summary>

        public int OrderId
        {
            get { return SluIOrderIdd; }
            set
            {
                if (SluIOrderIdd == value) return;
                SluIOrderIdd = value;
                RaisePropertyChanged(() => OrderId);
            }
        }

        public string SluDateTimeCtrl;

        /// <summary>
        /// 日 时:分  数据在控制器中生成的时间
        /// </summary>

        public string DateTimeCtrl
        {
            get { return SluDateTimeCtrl; }
            set
            {
                if (SluDateTimeCtrl == value) return;
                SluDateTimeCtrl = value;
                RaisePropertyChanged(() => DateTimeCtrl);
            }
        }

        public int SluIsdTemperature;

        /// <summary>
        /// 温度
        /// </summary>

        public int Temperature
        {
            get { return SluIsdTemperature; }
            set
            {
                if (SluIsdTemperature == value) return;
                SluIsdTemperature = value;
                RaisePropertyChanged(() => Temperature);
            }
        }

        public string SluStatus;

        /// <summary>
        /// 状态 0-正常，1-电压越上限，2-电压越下限，3-通讯故障
        /// </summary>

        public string Status
        {
            get { return SluStatus; }
            set
            {
                if (SluStatus == value) return;
                SluStatus = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public string SluIsAdjust;

        /// <summary>
        /// 已校准 0-未校准，1-已校准
        /// </summary>

        public string IsAdjust
        {
            get { return SluIsAdjust; }
            set
            {
                if (SluIsAdjust == value) return;
                SluIsAdjust = value;
                RaisePropertyChanged(() => IsAdjust);
            }
        }

        public string SlIsWorkingArgsSet;

        /// <summary>
        /// 工作参数设置 0-未设置，1-已设置
        /// </summary>

        public string IsWorkingArgsSet
        {
            get { return SlIsWorkingArgsSet; }
            set
            {
                if (SlIsWorkingArgsSet == value) return;
                SlIsWorkingArgsSet = value;
                RaisePropertyChanged(() => IsWorkingArgsSet);
            }
        }

        public string SlIsNoAlarm;

        /// <summary>
        /// 禁止主动报警 0-允许，1-禁止
        /// </summary>

        public string IsNoAlarm
        {
            get { return SlIsNoAlarm; }
            set
            {
                if (SlIsNoAlarm == value) return;
                SlIsNoAlarm = value;
                RaisePropertyChanged(() => IsNoAlarm);
            }
        }

        public string SlIsCtrlStop;

        /// <summary>
        /// 停运 0-正常，1-停运
        /// </summary>

        public string IsCtrlStop
        {
            get { return SlIsCtrlStop; }
            set
            {
                if (SlIsCtrlStop == value) return;
                SlIsCtrlStop = value;
                RaisePropertyChanged(() => IsCtrlStop);
            }
        }

        public string SIsEepromError;

        /// <summary>
        /// EEPROM故障 0-正常，1-故障
        /// </summary>

        public string IsEepromError
        {
            get { return SIsEepromError; }
            set
            {
                if (SIsEepromError == value) return;
                SIsEepromError = value;
                RaisePropertyChanged(() => IsEepromError);
            }
        }


        public string SIsTemperatureSensor;

        /// <summary>
        /// 温度传感器故障 0-正常，1-故障
        /// </summary>

        public string IsTemperatureSensor
        {
            get { return SIsTemperatureSensor; }
            set
            {
                if (SIsTemperatureSensor == value) return;
                SIsTemperatureSensor = value;
                RaisePropertyChanged(() => IsTemperatureSensor);
            }
        }


        #endregion

    }

    /// <summary>
    /// Ctrl Attri Ass 6
    /// </summary>
    public partial class NewDataViewModel
    {
        void UpdateCtrlData6(int sluId, int ctrlId)
        {
            SetSluNameId(sluId);
            this.CtrlId = ctrlId;
            this.CtrlPhyId = GetPhyIdByRtuId(sluId, ctrlId);
            this.LightDataItems.Clear();

            var info = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if(info ==null ||info .SluCtrlNewData ==null ||info .SluCtrlNewData .ContainsKey( ctrlId )==false )
 
            {
                ResetAllAttri6();
                return;
            }


            var tmp = info.SluCtrlNewData[ctrlId].DataAss6;
            if (tmp == null)
            {
                ResetAllAttri6();
                return;
            }
            DateCreate = new DateTime(tmp.DateTime).ToString("yyyy-MM-dd HH:mm:ss");
            LeakageCurrent = tmp.LeakageCurrent.ToString("f2");
            foreach (var t in tmp .LightDataField )
            {
                this.LightDataItems.Add(new LightDataVm(t));
            }

        }


        void ResetAllAttri6()
        {

            DateCreate = "----";
            LeakageCurrent = "--";

        }

        #region attri
        private string   _iLeakageCurrent;

        public string  LeakageCurrent
        {
            get { return _iLeakageCurrent; }
            set
            {
                if (_iLeakageCurrent == value) return;
                _iLeakageCurrent = value;
                RaisePropertyChanged(() => LeakageCurrent);
            }
        }


        private ObservableCollection<LightDataVm> _iLightDataItems;

        public ObservableCollection<LightDataVm> LightDataItems
        {
            get
            {
                if (_iLightDataItems == null)
                {
                    _iLightDataItems = new ObservableCollection<LightDataVm>();

                }
                return _iLightDataItems;
            }
            set
            {
                if (_iLightDataItems == value) return;
                _iLightDataItems = value;
                RaisePropertyChanged(() => LightDataItems);
            }
        }

        #endregion
    }

    /// <summary>
    /// 逻辑控制
    /// </summary>
    public partial class NewDataViewModel
    {
        private void ShowView()
        {
            Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                Wj2090Module.Services.ViewIdAssign.NewDataViewId);
        }

        private void OnSluDataArrive(List<int> types)
        {
            //  ShowView();

            if (types.Contains(4) || types.Contains(5) || types.Contains(6))
            {
                if (CurrentSelectedCtrlId != 0)
                {

                   
                    var info = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(CurrentSelectedRtuId);

                    if (info == null || info.SluCtrlNewData == null || info.SluCtrlNewData.ContainsKey(CurrentSelectedCtrlId) == false)
                    {
                        return;
                    }

                    CtrlMeasureInfo  tmps = info .SluCtrlNewData[CurrentSelectedCtrlId ]  ;
                    var dirr = DateTime.Now.Ticks - tmps.LastUpdateTime;
                    if (dirr > 30000000) return; //间隔三秒  该控制器数据未更新的
                    if (CurrentApplicationSelectd == CurrentSelectedRtuId)
                    {
                        ShowView();
                    }
                    if (types.Contains(4) && tmps.LastUpdate == 4)
                    {
                        UpdateCtrlData4(CurrentSelectedRtuId, CurrentSelectedCtrlId);
                        SelectedViewId = 4;
                        return;
                    }
                    if (types.Contains(6) && tmps.LastUpdate == 6)
                    {
                        UpdateCtrlData6(CurrentSelectedRtuId, CurrentSelectedCtrlId);
                        SelectedViewId = 6;
                        return;
                    }
                    if (types.Contains(5) && tmps.LastUpdate == 5)
                    {
                        UpdateCtrlData5(CurrentSelectedRtuId, CurrentSelectedCtrlId);
                        SelectedViewId = 5;
                        return;
                    }
                }
            }
            else
            {
                if (CurrentApplicationSelectd == CurrentSelectedRtuId)
                {
                    ShowView();
                }

                if (types.Contains(1))
                {
                    UpdateSluData(CurrentSelectedRtuId);
                    SelectedViewId = 1;
                    return;
                }
                if (types.Contains(2))
                {
                    UpdateSluDataUnkown2(CurrentSelectedRtuId);
                    SelectedViewId = 2;
                    return;
                }
            }

        }


        private void OnSelectRtu(int sluId, int ctrlid = 0)
        {
            if (CurrentSelectedRtuId == sluId)
            {
                CurrentSelectedCtrlId = ctrlid;
                if (ctrlid == 0)
                {
                    ShowView();
                    UpdateSluData(CurrentSelectedRtuId);
                    SelectedViewId = 1;
                }
                else
                {
                    ShowView();
                    UpdateCtrlData5(sluId, ctrlid);
                    SelectedViewId = 5;
                }
                return;
            }

            CurrentSelectedRtuId = sluId;
            CurrentSelectedCtrlId = ctrlid;

            ShowView();
            if (ctrlid == 0)
            {
                UpdateSluData(CurrentSelectedRtuId);
                SelectedViewId = 1;
            }
            else
            {
                UpdateCtrlData5(sluId, ctrlid);
                SelectedViewId = 5;
            }
        }
    }
}
