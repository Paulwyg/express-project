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
using Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.ViewModel
{



    [Export(typeof(IIEmergencyOperationNewData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyOperationNewDataViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                                               IIEmergencyOperationNewData
    {
        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "应急巡测"; }
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
        //界面是否开启
        private bool _isViewShow;
        public EmergencyOperationNewDataViewModel()
        {
            InitAction();
        }

        public void OnUserHideOrClosing()
        {
            //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = false;
            _isViewShow = false;

            TreeTmlNode.RegisterTmlNode.Clear();


            //throw new NotImplementedException();
        }

        public void NavInitBeforShow(params object[] parsObjects)
        {

        }

        public static double MinA = 0.5;
        protected List<int> LstPartolRtu = new List<int>();
        private Dictionary<int,List<int>> dic=new Dictionary<int, List<int>>(); 
        public void NavOnLoad(params object[] parsObjects)
        {
            //if (_isViewShow) return;

            _isViewShow = true;
            MeasurePatrolData.Clear();
            if (parsObjects.Count() == 0) return;
            var emergencyItems = parsObjects[0] as ObservableCollection<TreeNodeBase>;
            if(emergencyItems==null) return;
            int i = 0;
            foreach (var f in emergencyItems)
            {
                var tmps =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        f.NodeId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if(tmps==null) continue;
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(f.NodeId);
                if(run==null) continue;
                if(run.RtuNewData==null) continue;
                var grpname = "";
                var groupInfo = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(f.NodeId);
                if (groupInfo != null)
                {
                    var infosss =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                            GetGroupInfomation(
                                groupInfo.Item1, groupInfo.Item2);
                    if (infosss != null)
                    {
                        grpname = infosss.GroupName; // +" - " + infosss.GroupId;
                    }

                    //  if (infosss != null) DtRtuMsg += infosss.GroupName + " - ";

                }
                else
                {
                   grpname ="特殊终端";
                }
                foreach (var t in tmps.WjLoops)
                {
                    if ((t.Value.SwitchOutputId == 1 && f.K1SelectionTestAns != EnumSelectionTestAns.NoNeed) ||
                        t.Value.SwitchOutputId == 2 && f.K2SelectionTestAns != EnumSelectionTestAns.NoNeed ||
                        t.Value.SwitchOutputId == 3 && f.K3SelectionTestAns != EnumSelectionTestAns.NoNeed
                        || t.Value.SwitchOutputId == 4 && f.K4SelectionTestAns != EnumSelectionTestAns.NoNeed ||
                        t.Value.SwitchOutputId == 5 && f.K5SelectionTestAns != EnumSelectionTestAns.NoNeed ||
                        t.Value.SwitchOutputId == 6 && f.K6SelectionTestAns != EnumSelectionTestAns.NoNeed
                        || t.Value.SwitchOutputId == 7 && f.K7SelectionTestAns != EnumSelectionTestAns.NoNeed ||
                        t.Value.SwitchOutputId == 8 && f.K8SelectionTestAns != EnumSelectionTestAns.NoNeed)
                    {
                        foreach (var l in run.RtuNewData.LstNewLoopsData)
                        {
                            if (l.LoopId == t.Value.LoopId)
                            {
                                i++;
                                MeasurePatrolData.Add(new NewDataItems()
                                                          {
                                                              Index = i,
                                                              PhysicalId = f.PhysicalId,
                                                              RtuId = f.NodeId,
                                                              RtuName = f.NodeName,
                                                              LoopId = t.Value.LoopId,
                                                              LoopName = t.Value.LoopName,
                                                              SwitchInState = l.BolSwitchInState?"吸合":"断开",
                                                              Current = l.A.ToString("f2"),
                                                              Voltage = l.V.ToString("f2"),
                                                              ReceiveNewDataTime =
                                                                  run.RtuNewData.DateCreate.ToString(
                                                                      "yyyy-MM-dd HH:mm:ss"),
                                                              GroupName = grpname,
                                                              State = l.A < MinA ? "正常" : "异常",
                                                              Remarks = f.Remarks,
                                                              Color = l.A < MinA ? "#000000" : "#FF3030"
                                                          });
                            }
                        }

                    }
                }
            }
            //newdata.Clear();
            //foreach (var f in MeasurePatrolData)
            //{
            //    newdata.Add(f);
            //}
        }

        #region MeasurePatrolData

        private ObservableCollection<NewDataItems> _measurePatrolData;

        /// <summary>
        /// 应急巡测列表
        /// </summary>
        public ObservableCollection<NewDataItems> MeasurePatrolData
        {
            get { return _measurePatrolData ?? (_measurePatrolData = new ObservableCollection<NewDataItems>()); }
        }

        #endregion

        #region SumPartolTmlNumer
        private int _sumPartolTmlNumber;
        /// <summary>
        /// 进行召测的实际终端数量
        /// </summary>
        public int SumPartolTmlNumer
        {
            get { return _sumPartolTmlNumber; }
            private set
            {
                if (_sumPartolTmlNumber == value) return;
                _sumPartolTmlNumber = value;
                this.RaisePropertyChanged(() => this.SumPartolTmlNumer);
            }
        }
        #endregion

        #region SumAnswerPartolTmlNumber
        private int _sumAnswerPartolTmlNumber;

        /// <summary>
        /// 召测应答终端数量
        /// </summary>
        public int SumAnswerPartolTmlNumber
        {
            get { return _sumAnswerPartolTmlNumber; }
            private set
            {
                if (_sumAnswerPartolTmlNumber == value) return;
                _sumAnswerPartolTmlNumber = value;
                this.RaisePropertyChanged(() => this.SumAnswerPartolTmlNumber);
            }
        }
        #endregion

        #region AnsRemind

        private string _remindans;

        public string AnsRemind
        {
            get { return _remindans; }
            set
            {
                if (value == _remindans) return;
                _remindans = value;
                this.RaisePropertyChanged(() => this.AnsRemind);
            }
        }

        #endregion

        #region IsAbnormalChecked
        //public ObservableCollection<NewDataItems> newdata = new ObservableCollection<NewDataItems>();
        //private bool _isAbnormalChecked;

        //public bool IsAbnormalChecked
        //{
        //    get { return _isAbnormalChecked; }
        //    set
        //    {
        //        if (_isAbnormalChecked != value)
        //        {
        //            _isAbnormalChecked = value;
        //            MeasurePatrolData.Clear();
        //            if (_isAbnormalChecked)
        //            {
        //                foreach (var t in newdata)
        //                {
        //                    if (t.State == "异常")
        //                        MeasurePatrolData.Add(t);
        //                }
        //            }
        //            else
        //            {
        //                foreach (var t in newdata)
        //                {
        //                    MeasurePatrolData.Add(t);
        //                }
        //            }
        //            RaisePropertyChanged(() => IsAbnormalChecked);
        //        }
        //    }
        //}
        #endregion

    }

    public partial class EmergencyOperationNewDataViewModel
    {
        #region CmdPatrolEmergency

        private DateTime _dtCmdPatrolEmergency;
        private ICommand _cmdPatrolEmergency;

        /// <summary>
        /// 巡测应急终端
        /// </summary>
        public ICommand CmdPatrolEmergency
        {
            get
            {
                if (_cmdPatrolEmergency == null)
                    _cmdPatrolEmergency = new RelayCommand(ExCmdPatrolEmergency, CanCmdPatrolEmergency, false);
                return _cmdPatrolEmergency;
            }
        }

        private void ExCmdPatrolEmergency()
        {
            _dtCmdPatrolEmergency = DateTime.Now;
            LstPartolRtu.Clear();
            foreach (var f in MeasurePatrolData)
            {
                if (!LstPartolRtu.Contains(f.RtuId))
                    LstPartolRtu.Add(f.RtuId);
            }
            foreach (var t in MeasurePatrolData)
            {
                t.Current = "--";
                t.Voltage = "--";
                t.ReceiveNewDataTime = "--";
                t.SwitchInState = "--";
                t.State = "--";
            }
            SumPartolTmlNumer = LstPartolRtu.Count;
            SumAnswerPartolTmlNumber = 0;
            SndPartol();
        }
        private bool CanCmdPatrolEmergency()
         {
             return DateTime.Now.Ticks - _dtCmdPatrolEmergency.Ticks > 30000000;
         }
        #endregion

        #region CmdRePatrolEmergency

        private DateTime _dtCmdRePatrolEmergency;
        private ICommand _cmdRePatrolEmergency;

        /// <summary>
        /// 补测应急终端
        /// </summary>
        public ICommand CmdRePatrolEmergency
        {
            get
            {
                if (_cmdRePatrolEmergency == null)
                    _cmdRePatrolEmergency = new RelayCommand(ExCmdRePatrolEmergency, CanCmdRePatrolEmergency, false);
                return _cmdRePatrolEmergency;
            }
        }

        private void ExCmdRePatrolEmergency()
        {
            _dtCmdPatrolEmergency = DateTime.Now;
            _dtCmdRePatrolEmergency = DateTime.Now;
            SndPartol();
        }
        private bool CanCmdRePatrolEmergency()
        {
            if (DateTime.Now.Ticks - _dtCmdPatrolEmergency.Ticks > 30000000) return false;
            if (DateTime.Now.Ticks - _dtCmdRePatrolEmergency.Ticks < 150000000) return false;
            return LstPartolRtu.Count > 0;
        }
        #endregion

        #region CmdExport

        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        /// <summary>
        /// 导出
        /// </summary>
        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanCmdExport, false);
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
                lsttitle.Add("地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("回路");
                lsttitle.Add("回路名称");
                lsttitle.Add("接触器状态");
                lsttitle.Add("电流");
                lsttitle.Add("电压");
                lsttitle.Add("采集时间");
                lsttitle.Add("归属组");
                lsttitle.Add("来源");
                lsttitle.Add("状态");


                var lstobj = new List<List<object>>();
                foreach (var g in MeasurePatrolData)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhysicalId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.LoopId);
                    tmp.Add(g.LoopName);
                    tmp.Add(g.SwitchInState);
                    tmp.Add(g.Current);
                    tmp.Add(g.Voltage);
                    tmp.Add(g.ReceiveNewDataTime);
                    tmp.Add(g.GroupName);
                    tmp.Add(g.Remarks);
                    tmp.Add(g.State);

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanCmdExport()
        {
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

         private void SndPartol()
         {
             var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
             //info.Args .Addr .AddRange(LstPartolRtu);
             info.WstRtuOrders.Op = 31;
             info.WstRtuOrders.RtuIds.AddRange(LstPartolRtu);
             SndOrderServer.OrderSnd(info);
         }
    }

    public partial class EmergencyOperationNewDataViewModel
    {
        private void InitAction()
        {
            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg,
            //                              ResponseRtusInEmergency, typeof (LnEmergencyOperationCenterViewModel), this,
            //                              true);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders, // .wlst_svr_ans_cnt_request_wj3090_measure ,
                ResponseRtusInEmergency,
                typeof(EmergencyOperationNewDataViewModel), this);

        }



        private void ResponseRtusInEmergency(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            //var info = infos.WstRtutimeTimeTableEmerg;
            var info = infos.WstRtuOrders;
            if (info == null) return;
            foreach (var f in info.Items)
            {
                foreach (var l in MeasurePatrolData)
                {
                    foreach (var t in f.LstNewLoopsData)
                    {
                        if (l.RtuId == f.RtuId && t.LoopId == l.LoopId)
                        {
                            l.Current = t.A.ToString("f2");
                            l.Voltage = t.V.ToString("f2");
                            l.SwitchInState = t.SwitchInState ? "吸合" : "断开";
                            l.State = t.A < MinA ? "正常" : "异常";
                            l.ReceiveNewDataTime = new DateTime(f.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                            l.Color = t.A < MinA ? "#000000" : "#FF3030";
                        }
                    }
                }
                if (LstPartolRtu.Contains(f.RtuId))
                {
                    LstPartolRtu.Remove(f.RtuId);
                    SumAnswerPartolTmlNumber++;
                }
            }
            //MeasurePatrolData.Clear();
            //if(IsAbnormalChecked)
            //{
            //    foreach (var t in newdata)
            //    {
            //        if (t.State == "异常")
            //            MeasurePatrolData.Add(t);
            //    }
            //}
            //else
            //{
            //    foreach (var t in newdata)
            //    {
            //        MeasurePatrolData.Add(t);
            //    }
            //}

            if (LstPartolRtu.Count == 0)
                AnsRemind = DateTime.Now + "  " + "巡测完成.";
            else
                AnsRemind = DateTime.Now + "  " + "正在巡测...";
        }
    }

    public class NewDataItems : EventHandlerHelperExtendNotifyProperyChanged
    {
        #region Index

        private int _index;

        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (value == _index) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        #endregion

        #region PhysicalId

        private int _physicalId;

        /// <summary>
        /// 地址
        /// </summary>
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if (value == _physicalId) return;
                _physicalId = value;
                RaisePropertyChanged(() => PhysicalId);
            }
        }

        #endregion

        #region RtuId

        private int _rtuId;

        /// <summary>
        /// 逻辑地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        #endregion

        #region RtuName

        private string _rtuName;

        /// <summary>
        ///终端名称
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        #endregion

        #region LoopId

        private int _loopId;

        /// <summary>
        /// 回路
        /// </summary>
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (value == _loopId) return;
                _loopId = value;
                RaisePropertyChanged(() => LoopId);
            }
        }

        #endregion

        #region LoopName

        private string _loopName;

        /// <summary>
        ///回路名称
        /// </summary>
        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName != value)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        #endregion

        #region GroupName

        private string _groupName;

        /// <summary>
        ///归属组
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (_groupName != value)
                {
                    _groupName = value;
                    this.RaisePropertyChanged(() => this.GroupName);
                }
            }
        }

        #endregion

        #region ReceiveNewDataTime

        private string _receiveNewDataTime;
        /// <summary>
        /// 接收最新数据时间
        /// </summary>

        public string ReceiveNewDataTime
        {
            get { return _receiveNewDataTime; }
            set
            {
                if (_receiveNewDataTime == value) return;
                _receiveNewDataTime = value;
                this.RaisePropertyChanged(() => this.ReceiveNewDataTime);
            }
        }

        #endregion

        #region State

        private string _state;
        /// <summary>
        /// 状态
        /// </summary>

        public string State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                this.RaisePropertyChanged(() => this.State);
            }
        }

        #endregion

        #region Current

        private string _current;

        /// <summary>
        /// 电流
        /// </summary>
        public string Current
        {
            get { return _current; }
            set
            {
                if (value == _current) return;
                _current = value;
                RaisePropertyChanged(() => Current);
            }
        }

        #endregion

        #region Voltage

        private string _voltage;

        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage
        {
            get { return _voltage; }
            set
            {
                if (value == _voltage) return;
                _voltage = value;
                RaisePropertyChanged(() => Voltage);
            }
        }

        #endregion

        #region Remarks
        private string _remarks;

        /// <summary>
        /// 节点备注，是满足什么条件 所需要应急关灯
        /// </summary>
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    this.RaisePropertyChanged(() => this.Remarks);
                }
            }
        }
        #endregion

        #region Color
        private string _color;
        /// <summary>
        /// 新故障标红
        /// </summary>
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }
        #endregion

        #region SwitchInState

        private string _switchInState;

        /// <summary>
        ///接触器状态
        /// </summary>
        public string SwitchInState
        {
            get { return _switchInState; }
            set
            {
                if (_switchInState != value)
                {
                    _switchInState = value;
                    this.RaisePropertyChanged(() => this.SwitchInState);
                }
            }
        }

        #endregion
    }

}
