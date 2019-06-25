using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj3005;
using Wlst.Sr.ProtocolCnt.Wj1090;
using Wlst.Ux.Wj1090Module.Wj1090DataSelection.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090DataSelection.ViewModel
{
    [Export(typeof(IIWj1090DataSelection))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090DataSelectionViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIWj1090DataSelection
    {
        #region IITab
        public string Title
        {
            get { return string.Format("{0}", "数据选测"); }
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

        public Wj1090DataSelectionViewModel()
        {
            InitAction();
            InitEvent();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            var tmlId = (int)parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
            }
        }

        public void OnUserHideOrClosing()
        {
            SeleteDataItems.Clear();
            ReadLightRateItems.Clear();
        }
    }

    /// <summary>
    /// attr ,ICommand
    /// </summary>
     public partial class Wj1090DataSelectionViewModel
     {

         #region Attri

         #region AttachRtuId

         private int _attachRtuId;
         public int AttachRtuId
         {
             get { return _attachRtuId; }
             set
             {
                 if (_attachRtuId == value) return;
                 _attachRtuId = value;
                 RaisePropertyChanged(() => AttachRtuId);

                 AttachRtuName = "Reserve";
                 if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(_attachRtuId))
                     return;

                 var t = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[_attachRtuId] as Wj3005TerminalInformation;
                 if (t == null) return;
                 AttachRtuName = t.RtuName;
                 AttachPhyId = t.PhyId;
             }
         }

         private int _attachPhyId;

         public int AttachPhyId
         {
             get { return _attachPhyId; }
             set
             {
                 if (_attachPhyId == value) return;
                 _attachPhyId = value;
                 RaisePropertyChanged(() => AttachPhyId);
             }
         }
         #endregion

         #region AttachRtuName

         private string _attachRtuName;
         public string AttachRtuName
         {
             get { return _attachRtuName; }
             set
             {
                 if (_attachRtuName == value) return;
                 _attachRtuName = value;
                 RaisePropertyChanged(() => AttachRtuName);
             }
         }

         #endregion

         #region RemindSelectData
         private string _remindSelectData;
         public string RemindSelectData
         {
             get { return _remindSelectData; }
             set
             {
                 if (_remindSelectData == value) return;
                 _remindSelectData = value;
                 RaisePropertyChanged(() => RemindSelectData);
             }
         }
         #endregion

         #region RtuId

         private int _rtuId;
         public int RtuId
         {
             get { return _rtuId; }
             set
             {
                 if (_rtuId.Equals(value)) return;
                 _rtuId = value;
                 RaisePropertyChanged(() => RtuId);
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
                 if (_rtuName == value) return;
                 _rtuName = value;
                 RaisePropertyChanged(() => RtuName);
             }
         }

         #endregion

         #region LoopId
         private int _loopId;
         public int LoopId
         {
             get { return _loopId; }
             set
             {
                 if (_loopId == value) return;
                 _loopId = value;
                 RaisePropertyChanged("LoopId");
             }
         }
         #endregion

         #region PhyId

         private int _phtId;
         public int PhyId
         {
             get { return _phtId; }
             set
             {
                 if (_phtId.Equals(value)) return;
                 _phtId = value;
                 RaisePropertyChanged(() => PhyId);
             }
         }

         #endregion

         #region SelectVisi

         private bool _selectVisi;
         public bool SelectVisi
         {
             get { return _selectVisi; }
             set
             {
                 if (_selectVisi == value) return;
                 _selectVisi = value;
                 RaisePropertyChanged(() => SelectVisi);
             }
         }

         #endregion

         #region LightRateVisi
         private bool _lightRateVisi;
         public bool LightRateVisi
         {
             get { return _lightRateVisi; }
             set
             {
                 if (_lightRateVisi == value) return;
                 _lightRateVisi = value;
                 RaisePropertyChanged(() => LightRateVisi);
             }
         }
         #endregion

         #region SeleteDataItems

         private ObservableCollection<SelectDataModel> _selectDataItems;
         public ObservableCollection<SelectDataModel> SeleteDataItems
         {
             get { return _selectDataItems ?? (_selectDataItems = new ObservableCollection<SelectDataModel>()); }
         }
         #endregion

         #region ReadLightRateItems
         private ObservableCollection<ReadLightRateModel> _readLightRateItems;
         public ObservableCollection<ReadLightRateModel> ReadLightRateItems
         {
             get { return _readLightRateItems ?? (_readLightRateItems = new ObservableCollection<ReadLightRateModel>()); }
         }
         #endregion

         #region IsLock

        private bool _isLock;
        public bool IsLock
        {
            get { return _isLock; }
            set
            {
                if(_isLock==value) return;
                _isLock = value;
                RaisePropertyChanged(()=>IsLock);
            }
        }
         #endregion

         #endregion

         #region ICommand
         #region CmdSetLightOnRate

         private DateTime _dtSetLightOnRate;
         private ICommand _cmdSetLightOnRate;
         public ICommand CmdSetLightOnRate
         {
             get { return _cmdSetLightOnRate ?? (_cmdSetLightOnRate = new RelayCommand(ExBtnCmdSetLightOnRate, CanBtnCmdSetLightOnRate, true)); }
         }
         private void ExBtnCmdSetLightOnRate()
         {
             _dtSetLightOnRate = DateTime.Now;
             RemindSelectData = "设置亮灯率已命令...集中器ID："+RtuId+"线路ID："+LoopId+"请等待数据反馈";
             var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_SetBrightLightBase;
             info.Data.RtuId = RtuId;
             info.Data.ControlId = LoopId;
             SndOrderServer.OrderSnd(info, 10, 6);
         }
         private bool CanBtnCmdSetLightOnRate()
         {
             return DateTime.Now.Ticks - _dtSetLightOnRate.Ticks > 30000000;
         }
         #endregion

         #region CmdClearLightOnRate

         private DateTime _dtClearLightOnRate;
         private ICommand _cmdClearLightOnRate;
         public ICommand CmdClearLightOnRate
         {
             get { return _cmdClearLightOnRate ?? (_cmdClearLightOnRate = new RelayCommand(ExBtnCmdClearLightOnRate, CanBtnCmdClearLightOnRate, true)); }
         }
         private void ExBtnCmdClearLightOnRate()
         {
             _dtClearLightOnRate = DateTime.Now;
             RemindSelectData = "清除亮灯率命令已发送...集中器ID："+RtuId+"线路ID："+LoopId+"请等待数据反馈";
             var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ClearBrightLightBase;
             info.Data.RtuId = RtuId;
             info.Data.ControlId = LoopId;
             SndOrderServer.OrderSnd(info, 10, 6);
         }
         private bool CanBtnCmdClearLightOnRate()
         {
             return DateTime.Now.Ticks - _dtClearLightOnRate.Ticks > 30000000;
         }
         #endregion

         #region  Measure

         private DateTime _dtMeasureBtn;
         private ICommand _cmdMeasureBtn;
         public ICommand CmdMeasureBtn
         {
             get { return _cmdMeasureBtn ?? (_cmdMeasureBtn = new RelayCommand(ExCmdMeasureBtn, CanCmdMeasureBtn, true)); }
         }
         private bool CanCmdMeasureBtn()
         {
             return DateTime.Now.Ticks - _dtMeasureBtn.Ticks > 30000000;
         }
         private void ExCmdMeasureBtn()
         {
             SelectVisi = false;
             _dtMeasureBtn = DateTime.Now;
             SendMeasureOrder();
         }
         #endregion

         #region ReadBrightLight
         private DateTime _dtReadBrightLightBtn;
         private ICommand _cmdReadBrightLightBtn;
         public ICommand CmdReadBrightLightBtn
         {
             get { return _cmdReadBrightLightBtn ?? (_cmdReadBrightLightBtn = new RelayCommand(ExCmdReadBrightLightBtn, CanCmdReadBrightLightBtn, true)); }
         }
         private bool CanCmdReadBrightLightBtn()
         {
             return DateTime.Now.Ticks - _dtReadBrightLightBtn.Ticks > 30000000;
         }
         private void ExCmdReadBrightLightBtn()
         {
             _dtReadBrightLightBtn = DateTime.Now;
             LightRateVisi = false;
             SendReadBrightLightOrder();
         }
         #endregion

         #region CmdZcVersion
         private DateTime _dtCmdZcVersion;
         private ICommand _cmdCmdZcVersion;
         public ICommand CmdZcVersion
         {
             get { return _cmdCmdZcVersion ?? (_cmdCmdZcVersion = new RelayCommand(ExCmdCmdZcVersion, CanCmdCmdZcVersion, true)); }
         }
         private bool CanCmdCmdZcVersion()
         {
             return DateTime.Now.Ticks - _dtCmdZcVersion.Ticks > 30000000;
         }
         private void ExCmdCmdZcVersion()
         {
             _dtCmdZcVersion = DateTime.Now;
             SendZcVersionOrder();
         }
         #endregion
         
         #endregion
     }

    public partial class Wj1090DataSelectionViewModel
    {
        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId]
                as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;

            if (t == null)  return;
            var ffff = t.Clone();
            var tmp = ffff as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
            //属性自动生成
            if (tmp == null) return;
            RtuId = tmp.RtuId;
            PhyId = tmp.PhyId;
            RtuName = tmp.RtuName;
            LoopId = 0;
        }

        private void SendMeasureOrder()
        {
            SeleteDataItems.Clear();
            RemindSelectData = "选测命令已发送...集中器ID："+RtuId+"线路ID："+LoopId+"请等待数据反馈";
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_Measure;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = LoopId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SendReadBrightLightOrder()
        {
            ReadLightRateItems.Clear();
            RemindSelectData = "读取亮灯率命令已发送...集中器ID："+RtuId+"线路ID："+LoopId+"请等待数据反馈";
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ReadBrightLight;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = LoopId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SendZcVersionOrder()
        {

            RemindSelectData = "读取软件版本命令已发送...请等待数据反馈";
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ZcVersion;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }  
    }

    public partial class Wj1090DataSelectionViewModel
    {
        public void InitEvent()
        {
            EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ClearBrightLightBase,
               GetRecClearBrightLightData , typeof(Wj1090DataSelectionViewModel), this);

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetBrightLightBase,
               GetRecSetBrightLightData, typeof(Wj1090DataSelectionViewModel), this);

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_Measure,
             GetRecMeasureData, typeof(Wj1090DataSelectionViewModel), this);

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadBrightLight,
             GetRecReadBrightLightData, typeof(Wj1090DataSelectionViewModel), this);

            //ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetPars,
            // GetRecSetParData, typeof(Wj1090DataSelectionViewModel), this);

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ZcVersion,
             GetZcVersion, typeof(Wj1090DataSelectionViewModel), this);
        }

        private void GetRecSetBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
        {
            var info = infos.AddrLst;
            if (info.Count < 1) return;
            try
            {
                //var rtuId = infos.AddrLst[0];
                var controlId = infos.AddrLst[0];
                RemindSelectData = "编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "的集中器设置亮灯率成功！";
                // VisiRecSuccResponseFlag = Visibility.Visible;
                //LogInfo.Log("编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "的集中器清除亮灯率成功！");
            }
            catch (Exception e)
            {
                //LogInfo.Log("WJ1090清除亮灯率异常，" + e);
                RemindSelectData = "WJ1090设置亮灯率异常";
                // VisiRecSuccResponseFlag = Visibility.Visible;
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
                RemindSelectData = "编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "的集中器设清除灯率成功！";
                // VisiRecSuccResponseFlag = Visibility.Visible;

            }
            catch (Exception)
            {
                RemindSelectData = "WJ1090清除亮灯率异常";
                //VisiRecSuccResponseFlag = Visibility.Visible;
            }


        }

        private void GetZcVersion(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduVersion> infos)
        {
            // var info = infos.AddrLst;
            if (infos.Data == null) return;
            try
            {
                //var rtuId = infos.AddrLst[0];
                // var controlId = infos.AddrLst[0];
               
                RemindSelectData = infos.Data.RtuId + " 集中器软件版本为：" + infos.Data.Version;
                // VisiRecSuccResponseFlag = Visibility.Visible;

            }
            catch (Exception)
            {
                //OrderSndState = "WJ1090设置亮灯率异常";
                //VisiRecSuccResponseFlag = Visibility.Visible;
            }


        }

        //选测数据
        private void GetRecMeasureData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduMeasureData> infos)
        {
            var info = infos.Data;
            if (info == null) return;
            var tmps = (from t in info.Items where t.RtuId == RtuId select t).ToList();
            if (tmps.Count == 0) return;

            foreach (LduLineData item in tmps)
            {
                SeleteDataItems.Add(new SelectDataModel(item));
            }
            SelectVisi = true;
            RemindSelectData = "选测数据已反馈，请查看数据！";
        }
        //读取亮灯率数据
        private void GetRecReadBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduBrightLightData> infos)
        {
            var info = infos.Data;
            if (info == null) return;
            if(info.RtuId !=RtuId) return;
            ReadLightRateItems.Clear();

           
            if (infos.Data .RtuId !=RtuId ) return;
            foreach (var item in info.Items)
            {
                ReadLightRateItems.Add(new ReadLightRateModel(item));
            }
            LightRateVisi = true;
            RemindSelectData = "读取亮灯率数据已反馈，请查看数据！";
        }
        //设置参数
        private void GetRecSetParData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
        {
            var info = infos.AddrLst;
            if (info.Count < 1) return;
            try
            {
                //var rtuId = infos.AddrLst[0];
                var controlId = infos.AddrLst[0];
                RemindSelectData = "编号为：" + controlId.ToString(CultureInfo.InvariantCulture) + "参数设置成功！";
            }
            catch (Exception e)
            {
                RemindSelectData = "WJ1090设置参数异常";
            }

        }
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
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
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected && !IsLock)
                {
                    int rtuid = Convert.ToInt32(args.GetParams()[0]);
                    if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuid)) return;

                    var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(rtuid);
                    if (info == null) return;
                    AttachRtuId = info.AttachRtuId == 0 ? rtuid : info.AttachRtuId;
                    RtuId = rtuid;
                    RtuName = info.RtuName;
                    SeleteDataItems.Clear();
                    ReadLightRateItems.Clear();
                    SelectVisi = false;
                    LightRateVisi = false;
                    if (args.GetParams().Count > 1)
                    {
                        try
                        {
                            if (args.GetParams().Count > 1)
                            {
                                LoopId = Convert.ToInt32(args.GetParams()[1]);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        LoopId = 0;
                    }
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("LduInfoSetViewModel error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
