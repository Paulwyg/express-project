using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Nr6005Module.UserOpenCloseInfo.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.Nr6005Module.UserOpenCloseInfo.ViewModel
{
    [Export(typeof(IIUserOcInfo))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class UserOcInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIUserOcInfo, Wlst.Cr.Core.CoreInterface.IITab
    {
        public UserOcInfoViewModel()
        {
            InitAction();
            InitEvent();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestUserOcInfo, 1);
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_user_oc_info,
                                          //.wlst_svr_ans_cnt_request_snd_rtu_time,
                                          //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          ResponseUserOcInfo, typeof (UserOcInfoViewModel), this);
        }
        public void InitEvent()
        {
            EventPublish.AddEventTokener(Assembly.GetExecutingAssembly().GetName().ToString(),
                                                        FundEventHandlers, FundOrderFilters,true);
        }


        /// <summary>
        ///  请求数据 触发点
        /// </summary>
        public void RequestUserOcInfo()
        {
            var infos = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_user_oc_info;
            infos.WstRtuUserOcInfo.Op = 1;
            SndOrderServer.OrderSnd(infos,10,6);
        }


        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core &&
                    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    return false;
                }
                if (args.EventType == PublishEventType.Core &&
                    args.EventId == EventIdAssign.RunningInfoUpdate2)
                {
                   
                    return true;
                }
                if (args.EventType == PublishEventType.ReCn)
                {

                    return true;
                }
            }
            catch (Exception ex)
            {
                // WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.ReCn)
                {
                    var infos = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_user_oc_info;
                    infos.WstRtuUserOcInfo.Op = 1;
                    SndOrderServer.OrderSnd(infos,10,6);
                    
                }

                if (args.EventId == EventIdAssign.RunningInfoUpdate2)  //todo
                {
                  
                    var lst = args.GetParams()[0] as List<int>;
                    if (lst == null || lst.Count == 0) return;

                    //需要删除的记录
                    List<OcInfoItems> recordssss = new List<OcInfoItems>();
                    
                    if ( ocinfos == null) return;

                    //遍历记录中是否存在
                    foreach (var g in ocinfos.Keys )
                    {
                        if (lst.Contains(g.Item1))
                        {
                            var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(g.Item1);
                            if (run == null || run.RtuNewData == null) return;
                            if(run.RtuNewData.IsSwitchOutAttraction.Count <g.Item2) continue;

                            if(run.RtuNewData.IsSwitchOutAttraction[g.Item2-1]!= (ocinfos[g].Op==1) )
                            {
                                ocinfos[g].Color = "Red";
                                recordssss.Add(ocinfos[g]);
                               
                                
                            }
                            else
                            {
                                ocinfos[g].Color = "Black";
                            }
                        }
                    }
                    if(recordssss.Count >0 )
                    {
                        foreach (var g in recordssss)
                        {
                            //var tu = new Tuple<int, int>(g.RtuId,g.LoopId);
                            //if(ocinfos.ContainsKey(tu))
                            //{
                            //    Records.Remove(ocinfos[tu]);
                             
                            //}
                            //Records.Insert(0,g);
                            if (Records.Contains(g)) Records.Remove(g);
                            Records.Insert(0, g);
                        }
                    }
                    

                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("Ldu  showdata error in FundEventHandlers:ex:" + xe);
            }
        }



        private Dictionary<Tuple<int, int>, OcInfoItems> ocinfos = new Dictionary<Tuple<int, int>, OcInfoItems>(); 
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="args"></param>
        private void ResponseUserOcInfo(string session, Wlst.mobile.MsgWithMobile args)
        {
            var datax = args.WstRtuUserOcInfo;
            if (datax == null || datax.Items == null) return;
            var lst = datax.Items;
            if (lst.Count == 0) return;
            
            //1 请求全部  先全清记录，重新添加
            if (datax.Op == 1)
            {
                //var lst = datax.Items;
                Records.Clear();
                ocinfos.Clear();

                foreach (var g in lst)
                {
                    var tu = new Tuple<int, int>(g.RtuId, g.LoopId);

                    var info = new OcInfoItems()
                                   {
                                       RtuId = g.RtuId,
                                       DateCreate = new DateTime(g.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                       Op = g.Op,
                                       UserName = g.UserName,
                                       LoopId = g.LoopId,
                                       Color =
                                           g.UserName != Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName
                                               ? "Red"
                                               : "Black",
                                   };
                    ////记录缓存
                    //if (!ocinfos.ContainsKey(tu))
                    //{
                    ocinfos.Add(tu, info);
                    //}

                    Records.Insert(0, info);
                }
            }
            else if (datax.Op == 2)  //op2   添加，先检查是否已经存在，存在先删除 再添加
            {

                foreach (var g in lst)
                {
                    var tu = new Tuple<int, int>(g.RtuId, g.LoopId);
                    //如果存在，先删除
                    if (ocinfos.ContainsKey(tu))
                    {
                        Records.Remove(ocinfos[tu]); //totest
                        ocinfos.Remove(tu);
                        //删除原来记录
                        //foreach (var f in Records)
                        //{
                        //    if (f.RtuId == g.RtuId && f.LoopId == g.LoopId)
                        //    {
                        //        Records.Remove(f);

                        //        break;
                        //    }
                        //}
                    }
                    var info = new OcInfoItems()
                                   {
                                       RtuId = g.RtuId,
                                       DateCreate = new DateTime(g.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                       Op = g.Op,
                                       UserName = g.UserName,
                                       LoopId = g.LoopId,
                                       Color =
                                           g.UserName != Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName
                                               ? "Red"
                                               : "Black",
                                   };

                    ocinfos.Add(tu, info);
                    Records.Insert(0, info);
                }
            }
            else if (datax.Op == 3) //op3  删除
            {
                //var lst = datax.Items;
                foreach (var g in lst)
                {
                    var tu = new Tuple<int, int>(g.RtuId, g.LoopId);
                    if (ocinfos.ContainsKey(tu))
                    {
                        Records.Remove(ocinfos[tu]);
                        ocinfos.Remove(tu);
                    }
                }
            }
        }

        private OcInfoItems _cr;

        public OcInfoItems CurrentItem
        {
            get { return _cr; }
            set
            {
                if (_cr != value)
                {
                    _cr = value;
                    this.RaisePropertyChanged(() => this.CurrentItem);
                }
            }
        }

        public void CurrentSelectItemDoubleClicked()
        {
            try
            {

                if (CurrentItem == null) return;
                if (CurrentItem.RtuId < 1000000) return;
                //if (CurrentItem.RtuId > 1100000) return;
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                };
                args.AddParams(CurrentItem.RtuId);
                EventPublish.PublishEvent(args);
            }
            catch (Exception ex) { }
        }


        private ObservableCollection<OcInfoItems> _records;

        /// <summary>
        ///用户开关灯操作纪律
        /// </summary>
        public ObservableCollection<OcInfoItems> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OcInfoItems>();
                return _records;
            }
        }


        #region tab
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
            get { return "用户开关灯操作"; }
        }

        #endregion


    }

    public class OcInfoItems:Wlst.Cr.Core.CoreServices.ObservableObject
    {

        #region attri

        private int _rtuId;

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);


                    var tmp =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                    if (tmp != null)
                    {
                        PhyId = tmp.RtuPhyId;
                        RtuName = tmp.RtuName;
                    }

                }
            }
        }

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


        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }



        private int _op;

        public int Op
        {
            get { return _op; }
            set
            {
                if (_op != value)
                {
                    _op = value;
                    this.RaisePropertyChanged(() => this.Op);
                    if (Op == 1) StrOp = "开灯";
                    if (Op == 2) StrOp = "关灯";
                }
            }
        }

        private string _switchOut;

        public string SwitchOutName
        {
            get { return _switchOut; }
            set
            {
                if (value != _switchOut)
                {
                    _switchOut = value;
                    this.RaisePropertyChanged(() => this.SwitchOutName);
                }
            }
        }

        private int _loopId;

        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId != value)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);


                    //var tmps =
                    //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    //        RtuId ]
                    //    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    //if (tmps != null)
                    //{
                    //    if (tmps.WjLoops.ContainsKey(_loopId))
                    //    {
                    //        SwitchOutName = tmps.WjLoops[_loopId].LoopName;
                    //    }
                    //    else
                    //    {
                    SwitchOutName = "K" + _loopId;
                    //}
                }

            }
        }




        private string _strOp;

        public string StrOp
        {
            get { return _strOp; }
            set
            {
                if (_strOp != value)
                {
                    _strOp = value;
                    this.RaisePropertyChanged(() => this.StrOp);
                }
            }
        }

        private string _dateCreate;

        public string DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (_dateCreate != value)
                {
                    _dateCreate = value;
                    this.RaisePropertyChanged(() => this.DateCreate);
                }
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    this.RaisePropertyChanged(() => this.UserName);
                }
            }
        }



        private string _Color;

        /// <summary>
        /// 数据显示颜色
        /// </summary>
        public string Color
        {
            get { return _Color; }
            set
            {
                if (value != _Color)
                {
                    _Color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }

        #endregion
    }
}
