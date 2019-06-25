using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj1090Module.Wj1090DataSelection.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj1090Module.Wj1090DataSelection.ViewModel
{
    [Export(typeof(IIWj1090DataSelection))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090DataSelectionViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIWj1090DataSelection
    {
        #region IITab
        public int Index
        {
            get { return 1; }
        }
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
                 if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_attachRtuId))
                     return;

                 var t = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_attachRtuId] as Wj3005Rtu;
                 if (t == null) return;
                 AttachRtuName = t.RtuName;
                 AttachPhyId = t.RtuPhyId;
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
             RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  设置亮灯率已命令...集中器ID：" + RtuId + "线路ID：" + LoopId + "请等待数据反馈";
             var info = Sr.ProtocolPhone .LxLdu .wst_ldu_orders ;// .wlst_cnt_wj1090_order_set_bright_light ;//.ServerPart.wlst_Wj1090_clinet_order_SetBrightLightBase;
             info.WstLduOrders .LduId   = RtuId;
             info.WstLduOrders .LineIds .Add( LoopId );// .ControlId = LoopId;
             info.WstLduOrders.Op = 12;
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
             RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  清除亮灯率命令已发送...集中器ID：" + RtuId + "线路ID：" + LoopId + "请等待数据反馈";
             var info = Sr.ProtocolPhone .LxLdu .wst_ldu_orders ;
             info.WstLduOrders .LduId  = RtuId;
             info.WstLduOrders .LineIds .Add( LoopId );
             info.WstLduOrders.Op = 13;
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
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId]
                as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;

            if (t == null)  return;
            //var ffff = t.Clone();
            //var tmp = ffff as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
            //属性自动生成
            //if (tmp == null) return;
            RtuId = t.RtuId;
            var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuFid);
            if (rtuInfo!=null )
            {
                PhyId = rtuInfo.RtuPhyId;
                RtuName = rtuInfo.RtuName;
            }
            else
            {
                PhyId = t.RtuFid;
                RtuName = t.RtuName;
            }
            LoopId = t.RtuPhyId;
        }

        private void SendMeasureOrder()
        {
            SeleteDataItems.Clear();
            RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "选测命令已发送...集中器ID：" + RtuId + "线路ID：" + LoopId + "请等待数据反馈";
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_wj1090_order_measure ;//.ServerPart.wlst_Wj1090_clinet_order_Measure;
            //info.WstCntWj1090Orders.RtuId = RtuId;
            //info.WstCntWj1090Orders.ControlId = LoopId;
            

            var info = Sr.ProtocolPhone.LxLdu.wst_ldu_orders;
            info.WstLduOrders.LduId = RtuId;
            info.WstLduOrders.LineIds.Add(0);
            info.WstLduOrders.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SendReadBrightLightOrder()
        {
            ReadLightRateItems.Clear();
            RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  读取亮灯率命令已发送...集中器ID：" + RtuId + "线路ID：" + LoopId + "请等待数据反馈";
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_wj1090_order_read_bright_light ;//.ServerPart.wlst_Wj1090_clinet_order_ReadBrightLight;
            //info.WstCntWj1090Orders.RtuId = RtuId;
            //info.WstCntWj1090Orders.ControlId = LoopId;
            //SndOrderServer.OrderSnd(info, 10, 6);

            var info = Sr.ProtocolPhone.LxLdu.wst_ldu_orders;
            info.WstLduOrders.LduId = RtuId;
            info.WstLduOrders.LineIds.Add(LoopId);
            info.WstLduOrders.Op = 4;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SendZcVersionOrder()
        {

            RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  读取软件版本命令已发送...请等待数据反馈";
            var info = Sr.ProtocolPhone.LxLdu.wst_ldu_orders;
            info.WstLduOrders.LduId = RtuId;
            info.WstLduOrders.LineIds.Add(LoopId);
            info.WstLduOrders.Op = 3;
            SndOrderServer.OrderSnd(info, 10, 6);
        }  
    }

    public partial class Wj1090DataSelectionViewModel
    {
        public void InitEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public void InitAction()
        {

            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_wj1090_order_clear_bright_light ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_ClearBrightLightBase,
            //   GetRecClearBrightLightData , typeof(Wj1090DataSelectionViewModel), this);

            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_wj1090_order_set_bright_light ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetBrightLightBase,
            //   GetRecSetBrightLightData, typeof(Wj1090DataSelectionViewModel), this);

            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_wj1090_order_measure ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_Measure,
            // GetRecMeasureData, typeof(Wj1090DataSelectionViewModel), this);

            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_wj1090_order_read_bright_light ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadBrightLight,
            // GetRecReadBrightLightData, typeof(Wj1090DataSelectionViewModel), this);

            ////ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetPars,
            //// GetRecSetParData, typeof(Wj1090DataSelectionViewModel), this);

            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_wj1090_order_zc_version ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_ZcVersion,
            // GetZcVersion, typeof(Wj1090DataSelectionViewModel), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxLdu .wst_svr_ans_ldu_orders ,//.wlst_svr_ans_cnt_wj1090_order_zc_version,
         OnOrderBack, typeof(Wj1090DataSelectionViewModel), this);
        }

        private void OnOrderBack(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var datax = infos.WstLduSvrAnsOrders;
            var controlId = datax.LduId ;
            if(datax .Op ==12)
            {
                RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  编号为：" + controlId + "的集中器设置亮灯率成功！";
            }
            if (datax.Op == 13)
            {
                RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  编号为：" + controlId + "的集中器设清除灯率成功！";
            }
            if (datax.Op == 3)
            {
                RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  "+ controlId + " 集中器软件版本为：" + datax.Version;
            }
            if (datax.Op == 1)
            {
                var tmps = (from t in datax.ItemsData  where t.LduId  == RtuId select t).ToList();
                if (tmps.Count == 0) return;

                foreach (LduLineData item in tmps)
                {
                    SeleteDataItems.Add(new SelectDataModel(item));
                }
                SelectVisi = true;
                RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  选测数据已反馈，请查看数据！";
            }
            if (datax.Op == 4)
            {
                if (controlId != RtuId) return;
                ReadLightRateItems.Clear();

                foreach (var item in datax .ItemsBrightLight )
                {
                    ReadLightRateItems.Add(new ReadLightRateModel(item));
                }
                LightRateVisi = true;
                RemindSelectData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  读取亮灯率数据已反馈，请查看数据！";
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

                    var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);//.GetEquipmentInfo(rtuid);
                    if (info == null) return;
                    AttachRtuId = info.RtuFid == 0 ? rtuid : info.RtuFid;
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
