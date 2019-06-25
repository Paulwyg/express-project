using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj3005;
using Wlst.Sr.ProtocolCnt.AexchangeModels.ModelParts;
using Wlst.Ux.Wj1090Module.LduInfoSet.Services;
using Wlst.Sr.ProtocolCnt.Wj1090;

namespace Wlst.Ux.Wj1090Module.LduInfoSet.ViewModel
{
    /// <summary>
    /// 基本信息
    /// </summary>
    [Export(typeof(IILduInfoSetView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial  class LduInfoSetViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                        IILduInfoSetView
    {
        #region NavOnLoad

        public void NavOnLoad(params object[] parsObjects)
        {
            //初始化隐藏召测表格
            LvRecVisi=Visibility.Collapsed;
            RecLineItems.Clear();
            LineItems.Clear();
            var tmlId = (int) parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
            }
        }

        public void OnUserHideOrClosing()
        {
            RecLineItems.Clear();
            LineItems.Clear();
        }


        /// <summary>
        /// 提供外界更改终端
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId]
                as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;

            if (t == null)
                return;
            var ffff = t.Clone();
            var tmp = ffff as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
            //属性自动生成
            if (tmp == null)
                return;
            _terminalInformation = tmp;

            InitViewModel();
            if(LineItems .Count <3)
            {
                GridColum = 2;
                GridRow = 0;
                GridColumnSpan = 1;
            }
            else
            {
                GridColum = 0;
                GridRow = 2;
                GridColumnSpan =3;

            }
        }

        /// <summary>
        /// 初始化 需要显示的回路、输出、输入信息
        /// </summary>
        private void InitViewModel()
        {
            if (_terminalInformation == null)
                return;


            AttachRtuId = _terminalInformation.AttachRtuId;
            RtuId = _terminalInformation.RtuId;
            PhyId = _terminalInformation.PhyId;
            RtuName = _terminalInformation.RtuName;
            Remark = _terminalInformation.Remark;

            LduCommType = _terminalInformation.RtuModel == 30910 ? "无线" : "有线";

            LineItems.Clear();
            foreach (var t in _terminalInformation.LduLines)
            {
                LineItems.Add(new LduLineViewModel(t,LoopCollectionInfo));
            }

        }

        #endregion

        private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation _terminalInformation;

        /// <summary>
        /// 将回路信息、输入信、输出信息还原为 终端信息
        /// </summary>
        /// <returns></returns>
        private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation
            BackViewModelToTerminalInformation()
        {
            _terminalInformation.RtuName = RtuName;
            _terminalInformation.Remark = Remark;
            _terminalInformation.LduLines.Clear();
            foreach (var t in LineItems)
            {
                _terminalInformation.LduLines.Add(t.BackToLduLineParameter());
            }
            return _terminalInformation;
        }

        #region tab

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "参数设置"; }
        }
        


             private int _rGridColum;

        public int GridColum
        {
            get { return _rGridColum; }
            set
            {
                if (_rGridColum != value)
                {
                    _rGridColum = value;
                    RaisePropertyChanged(() => GridColum);
                }
            }
        }



        private int _rGridRow;

         public int GridRow
        {
            get { return _rGridRow; }
            set
            {
                if (_rGridRow != value)
                {
                    _rGridRow = value;
                    RaisePropertyChanged(() => GridRow);
                }
            }
        }


             private int _rGridColumnSet;

             public int GridColumnSpan
        {
            get { return _rGridColumnSet; }
            set
            {
                if (_rGridColumnSet != value)
                {
                    _rGridColumnSet = value;
                    RaisePropertyChanged(() => GridColumnSpan);
                }
            }
        }


   

        #endregion

        #region 基本数据

        private ObservableCollection<LduLineViewModel> _lineItems;

        /// <summary>
        /// 线路信息
        /// </summary>
        public ObservableCollection<LduLineViewModel> LineItems
        {
            get { return _lineItems ?? (_lineItems = new ObservableCollection<LduLineViewModel>()); }
        }

        private ObservableCollection<LduLineViewModel> _recLineItems;
        public ObservableCollection<LduLineViewModel> RecLineItems
        {
            get { return _recLineItems ?? (_recLineItems = new ObservableCollection<LduLineViewModel>()); }
        }


        private int _rtuId;

        /// <summary>
        /// 集中控制器地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId != value)
                {
                    _rtuId = value;
                    RaisePropertyChanged(() => RtuId);
                }
            }
        }


        private int _phyId;

        /// <summary>
        /// 集中控制器地址
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId != value)
                {
                    _phyId = value;
                    RaisePropertyChanged(() => PhyId);
                }
            }
        }

        private string _name;

        /// <summary>
        /// 集中控制器名称
        /// </summary>
        public string RtuName
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }


        private int _attachrtuId;

        /// <summary>
        /// 连接主设备地址
        /// </summary>
        public int AttachRtuId
        {
            get { return _attachrtuId; }
            set
            {
                if (_attachrtuId == value) return;
                _attachrtuId = value;
                RaisePropertyChanged(() => AttachRtuId);

                AttachRtuName = "Reserve";
                if (
                    !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                         EquipmentInfoDictionary.ContainsKey
                         (_attachrtuId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary
                        [_attachrtuId];
                AttachRtuName = tml.RtuName;

                var t =
    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[_attachrtuId]
         as Wj3005TerminalInformation;
                if(t==null) return;
                AttachRtuName = t.RtuName;
                LoopCollectionInfo.Clear();
                for (var i = 0; i <t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps().Count; i++)
                {
                  if(t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].SwitchOutId>0)
                  {
                      LoopCollectionInfo.Add(new NameValueInt
                      {
                          Name = t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].LoopName,
                          Value = t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].LoopId,

                      });   
                  }
                }
            }
        }

        private string _attachName;

        /// <summary>
        /// 连接主设备名称
        /// </summary>
        public string AttachRtuName
        {
            get { return _attachName; }
            set
            {
                if (_attachName == value) return;
                _attachName = value;
                RaisePropertyChanged(() => AttachRtuName);
            }
        }


        private string _lduCommType;

        /// <summary>
        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket  一般为3或6
        /// </summary>
        public string LduCommType
        {
            get { return _lduCommType; }
            set
            {
                if (_lduCommType != value)
                {
                    _lduCommType = value;
                    RaisePropertyChanged(() => LduCommType);
                }
            }
        }


        private string _remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }

        #region DataRecSuccResponseFlag

        private string _dataRecSuccResponseFlag;
        public string DataRecSuccResponseFlag
        {
            get
            {
                return _dataRecSuccResponseFlag;
            }
            set
            {
                if(_dataRecSuccResponseFlag==value) return;
                _dataRecSuccResponseFlag = value;
                RaisePropertyChanged(()=>DataRecSuccResponseFlag);
            }
        }
        #endregion

        #region VisiRecSuccResponseFlag

        private  Visibility _visiRecSuccResponseFlag;
        public Visibility VisiRecSuccResponseFlag
        {
            get
            {
                return _visiRecSuccResponseFlag;
            }
            set
            {
                if(_visiRecSuccResponseFlag==value) return;
                _visiRecSuccResponseFlag = value;
                RaisePropertyChanged(()=>VisiRecSuccResponseFlag);

            }
        }
        #endregion

        #region 终端的所有回路

        private List<NameValueInt> _loopCollectionInfo;
        public List<NameValueInt> LoopCollectionInfo
        {
            get { return _loopCollectionInfo ?? (_loopCollectionInfo = new List<NameValueInt>()); }
        }

        #endregion

        #endregion

        #region 招测基本数据

        /// <summary>
        /// 控制召测数据是否显示
        /// </summary>
        private Visibility _lVRecVisi;
        public Visibility LvRecVisi
        {
            get { return _lVRecVisi; }
            set
            {
                if(value!=_lVRecVisi)
                {
                    _lVRecVisi = value;
                    RaisePropertyChanged(() => LvRecVisi);
                }
            }
        }

        private bool _isHidenRecData;
        public bool IsHidenRecData
        {
            get { return _isHidenRecData; }
            set
            {
                if (value == _isHidenRecData) return;
                _isHidenRecData = value;
                if(_isHidenRecData)
                {
                    LvRecVisi = Visibility.Collapsed;
                }
                RaisePropertyChanged(()=>IsHidenRecData);
            }
        }



        #endregion

        #region ICommand

        private DateTime[] _dateTimes = new DateTime[4];

        #region CmdBtnZhaoCe
        private ICommand _cmdBtnZhaoCe;
        public ICommand CmdBtnZhaoCe
        {
            get { return _cmdBtnZhaoCe ?? (_cmdBtnZhaoCe = new RelayCommand(ExBtnZhaoCe, CanBtnZhaoCe, true)); }
        }
        private void ExBtnZhaoCe()
        {
            _dateTimes[0] = DateTime.Now;
            RequestParsData();
            RecLineItems.Clear();
            //foreach (var t in LineItems) RecLineItems.Add(t);
        }
        private bool CanBtnZhaoCe()
        {
            return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
        }
        #endregion 

        #region CmdSaveAndSnd
        private ICommand _cmdSaveAndSnd;
        public ICommand CmdSaveAndSnd
        {
            get { return _cmdSaveAndSnd ?? (_cmdSaveAndSnd = new RelayCommand(ExBtnSaveAndSnd, CanBtnSaveAndSnd, true)); }
        }
        private void ExBtnSaveAndSnd()
        {
            _dateTimes[1] = DateTime.Now;
            var ins = BackViewModelToTerminalInformation();
            if (ins == null) return;
            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.UpdateEquipmentInfo(ins);
            VisiRecSuccResponseFlag=Visibility.Collapsed;
            DataRecSuccResponseFlag = "";
        }
        private bool CanBtnSaveAndSnd()
        {
            return DateTime.Now.Ticks-_dateTimes[1].Ticks>30000000;
        }
        #endregion

        #region CmdSetLightOnRate
        private ICommand _cmdSetLightOnRate;
        public ICommand CmdSetLightOnRate
        {
            get { return _cmdSetLightOnRate ?? (_cmdSetLightOnRate = new RelayCommand(ExBtnCmdSetLightOnRate, CanBtnCmdSetLightOnRate, true)); }
        }
        private void ExBtnCmdSetLightOnRate()
        {
            _dateTimes[2] = DateTime.Now;
            DataRecSuccResponseFlag = "正在发送设置亮灯率命令!!!";
            VisiRecSuccResponseFlag=Visibility.Visible;
           // LogInfo.Log("正在发送设置亮灯率命令!!!");
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_SetBrightLightBase;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
        private bool CanBtnCmdSetLightOnRate()
        {
            return DateTime.Now.Ticks - _dateTimes[2].Ticks > 30000000;
        }
        #endregion
        #region CmdClearLightOnRate
        private ICommand _cmdClearLightOnRate;
        public ICommand CmdClearLightOnRate
        {
            get { return _cmdClearLightOnRate ?? (_cmdClearLightOnRate = new RelayCommand(ExBtnCmdClearLightOnRate, CanBtnCmdClearLightOnRate, true)); }
        }
        private void ExBtnCmdClearLightOnRate()
        {
            _dateTimes[3] = DateTime.Now;
            DataRecSuccResponseFlag = "正在发送清除亮灯率命令!!!";
            VisiRecSuccResponseFlag = Visibility.Visible;
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ClearBrightLightBase;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
        private bool CanBtnCmdClearLightOnRate()
        {
            return DateTime.Now.Ticks - _dateTimes[3].Ticks > 30000000;
        }
        #endregion
        #endregion


    }

    /// <summary>
    /// 构造函数
    /// </summary>
     public partial class LduInfoSetViewModel
     {
         public LduInfoSetViewModel()
         {
             _visiRecSuccResponseFlag = Visibility.Collapsed;
             InitAction();
             InitEvent();
         }
     }

  

     /// <summary>
     /// Event
     /// </summary>
     public partial class LduInfoSetViewModel
     {
         public void InitEvent()
         {
             EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
         }
         public void InitAction()
         {

             ProtocolServer.RegistProtocol(
                 Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadPars,
                 ResolveRequestParsData,
                 typeof (LduInfoSetViewModel), this);
             ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetPars,
                                           ResolveAnsSndParsData,
                                           typeof (LduInfoSetViewModel), this);
             ProtocolServer.RegistProtocol(
                 Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ClearBrightLightBase,
                 GetRecSetBrightLightData,
                 typeof (LduInfoSetViewModel), this);
             ProtocolServer.RegistProtocol(
                 Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetBrightLightBase,
                 GetRecClearBrightLightData,
                 typeof (LduInfoSetViewModel), this);
         }

         private void ResolveRequestParsData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduParsData> infos)
         {
             var info = infos.Data;
             if (info == null) return;
             if (RtuId != info.RtuId) return;  //如果是本客户端选测则继续运行下面代码

             //显示数据表格
             LvRecVisi = Visibility.Visible;
             IsHidenRecData = false;
           //  RecLineItems.Clear();
             foreach (LduLinePars item in info.Items)
             {
                 var lduLine = new LduLineParameter
                                   {
                                       AlarmAutoReport = item.AlarmAutoReport,
                                       AlarmLineBrightRate = item.AlarmLineBrightRate,
                                       AlarmLineLightOffImpedance = item.AlarmLineLightOffImpedance,
                                       AlarmLineLightOffSingle = item.AlarmLineLightOffSingle,
                                       AlarmLineLightOpenImpedance = item.AlarmLineLightOpenImpedance,
                                       AlarmLineLightOpenSingel = item.AlarmLineLightOpenSingel,
                                       AlarmLineLosePower = item.AlarmLineLosePower,
                                       AlarmLineShortCircuit = item.AlarmLineShortCircuit,
                                       LduBrightRateAlarmLimit = item.LduBrightRateAlarmLimit,
                                       LduEndLampportSn = item.LduEndLampportSn.ToString(CultureInfo.InvariantCulture),
                                       LduLightoffImpedanceLimit = item.LduLightoffImpedanceLimit,
                                       LduLightoffSingleLimit = item.LduLightoffSingleLimit,
                                       LduLightonImpedanceLimit = item.LduLightonImpedanceLimit,
                                       LduLightonSingleLimit = item.LduLightonSingleLimit,
                                       LduLoopID = item.LineLoopId,
                                       LduPhase = item.LduPhase,
                                       
                                       MutualInductorRadio = item.MutualInductorRadio
                                   };


                 RecLineItems.Add(new LduLineViewModel(lduLine,LoopCollectionInfo));

                 // 在计算机内存中读取回路对应的线路名称、线路ID等
                 foreach (var recLineItem in RecLineItems)
                 {
                     foreach (var line in _terminalInformation.LduLines)
                     {
                         if(recLineItem.LduLoopID==line.LduLoopID)
                         {
                             recLineItem.LduLineID = line.LduLineID;
                             recLineItem.LduLineName ="[招测] "+ line.LduLineName;
                             recLineItem.IsUsed = line.IsUsed;
                             recLineItem.Remark = line.Remark;
                             recLineItem.LduControlTypeCode = line.LduControlTypeCode;
                         }
                     }
                 }

             }
         }

         private  void ResolveAnsSndParsData(string session,Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
         {
             var info = infos.AddrLst;
             
             if (info == null) return;
             if (info.Count > 0)
                 DataRecSuccResponseFlag += "数据下发成功!!" + string.Format("{0:F}", DateTime.Now);

         }
         private void GetRecSetBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
         {
             var info = infos.AddrLst;
             if (info.Count < 1) return;
             try
             {
                 //var rtuId = infos.AddrLst[0];
                 var controlId = infos.AddrLst[0];
                 DataRecSuccResponseFlag = "编号为：" + controlId.ToString(CultureInfo.InvariantCulture)+ "的集中器清除亮灯率成功！";
                 VisiRecSuccResponseFlag = Visibility.Visible;
                 //LogInfo.Log("编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "的集中器清除亮灯率成功！");
             }
             catch (Exception e)
             {
                 //LogInfo.Log("WJ1090清除亮灯率异常，" + e);
                 DataRecSuccResponseFlag = "WJ1090清除亮灯率异常";
                 VisiRecSuccResponseFlag = Visibility.Visible;
             }


         }
         private void GetRecClearBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
         {
             var info = infos.AddrLst;
             if (info.Count < 1) return;
             try
             {
                 //var rtuId = infos.AddrLst[0];
                 var controlId = infos.AddrLst[0];
                 DataRecSuccResponseFlag = "编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "的集中器设置亮灯率成功！";
                 VisiRecSuccResponseFlag = Visibility.Visible;

             }
             catch (Exception)
             {
                 DataRecSuccResponseFlag = "WJ1090设置亮灯率异常";
                 VisiRecSuccResponseFlag = Visibility.Visible;
             }


         }

         public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
         {
             try
             {
                 if (args.EventType == PublishEventType.Core && args.EventId ==Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                 {
                     return true;
                 }
             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError("Error:" + ex);
             }
             return false;
         }

         public void FundEventHandlers(PublishEventArgs args)
         {
             try
             {
                 if (args.EventType == PublishEventType.Core && args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                 {
                     var temptuple = args.GetParams()[0];
                     if (temptuple == null) return;
                     var temp = temptuple as List<Tuple<int, int>>;
                     if (temp == null) return;
                     var id = temp[0].Item1;
                     DataRecSuccResponseFlag += "数据保存成功!!" + string.Format("{0:F}", DateTime.Now);
                     VisiRecSuccResponseFlag = id == RtuId ? Visibility.Visible : Visibility.Collapsed;

                 }

             }
             catch (Exception xe)
             {
                 WriteLog.WriteLogError("LduInfoSetViewModel error in FundEventHandlers:ex:" + xe);
             }
         }
     }

     /// <summary>
     /// Socket
     /// </summary>
     public partial class LduInfoSetViewModel
     {
         private void RequestParsData()
         {
          
             LogInfo.Log("正在请求所有类型信息!!!");
             var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ReadPars;        
             info.Data.RtuId = RtuId;
             info.Data.ControlId = 0;
             SndOrderServer.OrderSnd(info, 10, 6);
         }
     }

}
