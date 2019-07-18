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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.WJ3005Module.BatchStopRunning.Services;
using Wlst.Ux.WJ3005Module.BatchStopRunning.ViewModels;



namespace Wlst.Ux.WJ3005Module.BatchStopRunning.ViewModels
{
    [Export(typeof(IIBatchStopRunning))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial  class BatchStopRunning : EventHandlerHelperExtendNotifyProperyChanged,IIBatchStopRunning
    {
        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "批量停运"; }
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
        public BatchStopRunning()
        {
            //InitEvent();
            InitAction();
          
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            if (_isViewShow) return;
            _isViewShow = true;

            Items.Clear();

            OcCount = 0;
            OcCountAns = 0;
            OcTmlCount = 0;
            TimeAns = 0;

            if (parsObjects.Count() < 2) return;
            var rtus = parsObjects[0] as List<int>;
            if (rtus == null || rtus.Count == 0) return;
            AddTmls(rtus);

            var inttmp =Convert.ToInt16(parsObjects[1]) ;
            IsStopRun = inttmp == 0;
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,//.wlst_svr_ans_cnt_request_snd_rtu_time,
                //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          ResponseStopRunning, typeof(BatchStopRunning), this);

        }
        public void OnUserHideOrClosing()
        {
          //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = false;
            _isViewShow = false;
            Items.Clear();

            //throw new NotImplementedException();
        }

    }


    /// <summary>
    /// attri
    /// </summary>
    public partial class BatchStopRunning
    {
        #region Field
        /// <summary>
        /// 界面是否打开，不打开就不监听
        /// </summary>
        private bool _isViewShow;
        /// <summary>
        /// 记录操作的终端列表
        /// </summary>
        private List<int> _waitAnsTml = new List<int>();
        /// <summary>
        /// 记录最新的操作  0-无操作  1-停运  2-解除停运
        /// </summary>
        private int _lastOcOr = 0;

        #endregion

        #region OcCount


        private int _remindOcCount;
        public int OcCount
        {
            get { return _remindOcCount; }
            set
            {
                if (_remindOcCount == value) return;
                _remindOcCount = value;
                RaisePropertyChanged(() => OcCount);
            }
        }
        #endregion

        #region OcCountAns
        private int _remindOcCountOcCountAns;
        public int OcCountAns
        {
            get { return _remindOcCountOcCountAns; }
            set
            {
                if (_remindOcCountOcCountAns == value) return;
                _remindOcCountOcCountAns = value;
                RaisePropertyChanged(() => OcCountAns);
            }
        }
        #endregion

        #region OcTmlCount


        private int _remindOcTmlCount;
        public int OcTmlCount
        {
            get { return _remindOcTmlCount; }
            set
            {
                if (_remindOcTmlCount == value) return;
                _remindOcTmlCount = value;
                RaisePropertyChanged(() => OcTmlCount);
            }
        }
        #endregion

        #region TimeAns
        private double _remindOcCountOcCoIsShowOcCountAnsuntAns;
        public double TimeAns
        {
            get { return _remindOcCountOcCoIsShowOcCountAnsuntAns; }
            set
            {
                if (_remindOcCountOcCoIsShowOcCountAnsuntAns == value) return;
                _remindOcCountOcCoIsShowOcCountAnsuntAns = value;
                RaisePropertyChanged(() => TimeAns);
            }
        }
        #endregion
      
        #region Items

        private ObservableCollection<BatchStopRunningRecords> _items;
        public ObservableCollection<BatchStopRunningRecords> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<BatchStopRunningRecords>();
                }
                return _items;
            }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
            }
            
        }

        #endregion

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

    }


    /// <summary>
    /// Methods
    /// </summary>
    public partial class BatchStopRunning
    {

        private void AddTmls(List<int> rtus)
        {
            Items.Clear();
            _waitAnsTml.Clear();
            var tmpitems = new ObservableCollection<BatchStopRunningRecords>();
            foreach (var rtuid in rtus)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid)) continue;
                var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
                var tmp = new BatchStopRunningRecords
                              {
                                  RtuId =rtuid ,
                                  RtuName = rtuInfo.RtuName,
                                  RtuStateCode =rtuInfo.RtuStateCode,
                                  OpTime = DateTime.Now,
                                  AnsTime = null,
                                  Answer = null
                              };
                    tmpitems.Add(tmp);
                    _waitAnsTml.Add(rtuid);
                
            }
            this.Items = tmpitems;

        }

        private void ResponseStopRunning(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            var datax = infos.WstRtuOrders;
            if (datax == null) return;
            if (datax.Op ==6 )  //停运
            {
                if (_lastOcOr != 1) return;
                foreach (var f in datax.RtuIds)
                {
                    if (!_waitAnsTml.Contains(f)) continue;
                    foreach (var g in Items)
                    {
                        if(g.RtuId == f)
                        {
                            g.Answer = "停运成功";

                            _waitAnsTml.Remove(g.RtuId);
                            break;
                        }
                    }
                  
                }
                
            }else if(datax.Op ==7) //解除停运
            {
                if (_lastOcOr != 2) return;
                foreach (var f in datax.RtuIds)
                {
                    if (!_waitAnsTml.Contains(f)) continue;
                    foreach (var g in Items)
                    {
                        if (g.RtuId == f)
                        {
                            g.Answer = "解除停运成功";

                            _waitAnsTml.Remove(g.RtuId);
                            break;
                        }

                    }

                }
            }
        }



        #region CmdStopRun
        private DateTime _dtStopRun;
        private ICommand _cmdStopRun;
        public ICommand CmdStopRun
        {
            get { return _cmdStopRun ?? (_cmdStopRun = new RelayCommand(ExStopRun, CanStopRun, true)); }
        }
        private void ExStopRun()
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
            {
                var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                if (sss == UMessageBoxWantPassWord.CancelReturn)
                {
                    return;
                }
                if (sss != UserInfo.UserLoginInfo.UserPassword)
                {
                    UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                     UMessageBoxButton.Yes);
                    return;
                }
            }
            else
            {
                var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行停运操作，\r\n若确定请输入验证码:1234", "");
                if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                {
                    return;
                }

                if (sss != "1234")
                {
                    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                    return;
                }
            }

            var infoss = WlstMessageBox.Show("确认停运",
                                           "确认停运。", WlstMessageBoxType.YesNo);
            if (infoss == WlstMessageBoxResults.Yes)
            {
                _dtStopRun = DateTime.Now;
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(_waitAnsTml);
                info.WstRtuOrders.RtuIds.AddRange(_waitAnsTml);
                info.WstRtuOrders.Op = 6;
                SndOrderServer.OrderSnd(info, 10, 6);

                _lastOcOr = 1;
            }
            // lvf 2019年2月7日15:58:43   更新本地缓存
            foreach (var g in _waitAnsTml)
            {
                if ( Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false) continue;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuStateCode = 1;
            }

            
        }
        private bool CanStopRun()
        {
            return DateTime.Now.Ticks - _dtStopRun.Ticks > 30000000;
        }
        #endregion

        #region CmdReRun
        private DateTime _dtReRun;
        private ICommand _cmdReRun;
        public ICommand CmdReRun
        {
            get { return _cmdReRun ?? (_cmdReRun = new RelayCommand(ExReRun, CanReRun, true)); }
        }
        private void ExReRun()
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
            {
                var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                if (sss == UMessageBoxWantPassWord.CancelReturn)
                {
                    return;
                }
                if (sss != UserInfo.UserLoginInfo.UserPassword)
                {
                    UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                     UMessageBoxButton.Yes);
                    return;
                }
            }
            else
            {
                var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行解除停运操作，\r\n若确定请输入验证码:1234", "");
                if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                {
                    return;
                }

                if (sss != "1234")
                {
                    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                    return;
                }
            }

            var infoss = WlstMessageBox.Show("确认解除停运",
                                           "确认解除停运。", WlstMessageBoxType.YesNo);
            if (infoss == WlstMessageBoxResults.Yes)
            {
                _dtReRun = DateTime.Now;
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(_waitAnsTml);
                info.WstRtuOrders.RtuIds.AddRange(_waitAnsTml);
                info.WstRtuOrders.Op = 7;
                SndOrderServer.OrderSnd(info, 10, 6);
                _lastOcOr = 2;
            }

            // lvf 2019年2月7日15:58:43   更新本地缓存
            foreach (var g in _waitAnsTml)
            {
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false) continue;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuStateCode = 2;
            }

        }
        private bool CanReRun()
        {
            return DateTime.Now.Ticks - _dtReRun.Ticks > 30000000;
        }
        #endregion



        #region CmdStopRunAgain
        private DateTime _dtStopRunAgain;
        private ICommand _cmdStopRunAgain;
        public ICommand CmdStopRunAgain
        {
            get { return _cmdStopRunAgain ?? (_cmdStopRunAgain = new RelayCommand(ExStopRunAgain, CanStopRunAgain, true)); }
        }
        private void ExStopRunAgain()
        {
            if (_waitAnsTml.Count == 0)
            {
                UMessageBox.Show("提醒", "没有需要补发的设备", UMessageBoxButton.Ok);
                return;
            }
            var infoss = WlstMessageBox.Show("确认停运",
                                           "确认停运。", WlstMessageBoxType.YesNo);
            if (infoss == WlstMessageBoxResults.Yes)
            {
                _dtStopRunAgain = DateTime.Now;
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(_waitAnsTml);
                info.WstRtuOrders.RtuIds.AddRange(_waitAnsTml);
                info.WstRtuOrders.Op = 6;
                SndOrderServer.OrderSnd(info, 10, 6);

                _lastOcOr = 1;
            }
            // lvf 2019年2月7日15:58:43   更新本地缓存
            foreach (var g in _waitAnsTml)
            {
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false) continue;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuStateCode = 1;
            }


        }
        private bool CanStopRunAgain()
        {
            return DateTime.Now.Ticks - _dtStopRunAgain.Ticks > 30000000;
        }
        #endregion

        #region CmdReRunAgain
        private DateTime _dtReRunAgain;
        private ICommand _cmdReRunAgain;
        public ICommand CmdReRunAgain
        {
            get { return _cmdReRunAgain ?? (_cmdReRunAgain = new RelayCommand(ExReRunAgain, CanReRunAgain, true)); }
        }
        private void ExReRunAgain()
        {
            if (_waitAnsTml.Count == 0)
            {
                UMessageBox.Show("提醒", "没有需要补发的设备", UMessageBoxButton.Ok);
                return;
            }
            var infoss = WlstMessageBox.Show("确认解除停运",
                                           "确认解除停运。", WlstMessageBoxType.YesNo);
            if (infoss == WlstMessageBoxResults.Yes)
            {
                _dtReRunAgain = DateTime.Now;
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(_waitAnsTml);
                info.WstRtuOrders.RtuIds.AddRange(_waitAnsTml);
                info.WstRtuOrders.Op = 7;
                SndOrderServer.OrderSnd(info, 10, 6);
                _lastOcOr = 2;
            }

            // lvf 2019年2月7日15:58:43   更新本地缓存
            foreach (var g in _waitAnsTml)
            {
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false) continue;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuStateCode = 2;
            }

        }
        private bool CanReRunAgain()
        {
            return DateTime.Now.Ticks - _dtReRunAgain.Ticks > 30000000;
        }
        #endregion


    }
}
