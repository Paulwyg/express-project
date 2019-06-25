using System;
using System.Windows;
using System.Windows.Input;
using Lurx.Controls.EventSchedule;
using Lurx.Controls.EventScheduleView.Services;
using Wlst.client;

namespace Lurx.Controls.EventScheduleView.ViewModel
{
    [Export(typeof(IIWEventScheduleView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WjEventScheduleViewVm : EventTaskInstanceInfoViewModel,
                                                           IIWEventScheduleView
    {
        public WjEventScheduleViewVm()
        {
            this.InitAction() ;
        }

        #region  define

        private Visibility _txtTagw;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtTagw; }
            set
            {
                if (value != _txtTagw)
                {
                    _txtTagw = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }


  


        #endregion

        private DateTime [] _dateTimes=new DateTime[3];

        #region CmdSetPara

        private ICommand _cmdSetPara;

        public ICommand CmdSetPara
        {
            get
            {
                if (_cmdSetPara == null) _cmdSetPara = new RelayCommand(ExSetPara, CanExSetPara, true);
                return _cmdSetPara;
            }
        }


        private void ExSetPara()
        {
            _dateTimes[0] = DateTime.Now;
            Lurx.Controls.EventSchedule.EventScheduleWindow window = new EventScheduleWindow();
            window.SetDataContextEventScheduleViewModel(EventSchedule);
            window.OnSaveEventSchedule += new System.EventHandler(window_OnSaveEventSchedule);
            window.Show();

        }

        private void window_OnSaveEventSchedule(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            var send = sender as EventScheduleWindow;
            if (send == null) return;
            var ggg = send.DataContext as Wlst.client.EventSchedule;
            if (ggg != null) this.EventSchedule = ggg;
            send.Close();
        }

        private bool CanExSetPara()
        {
            return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
        }



        #endregion

        #region CmdSave

        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null) _cmdSave = new RelayCommand(ExSave, CanExSave, true);
                return _cmdSave;
            }
        }

        private void ExSave()
        {
            _dateTimes[1] = DateTime.Now;
            UpdatePartolEventTaskInstanceInfo();
            Visi = Visibility.Hidden;
        }

        private bool CanExSave()
        {
            return DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
        }

        #endregion


        #region CmdSeeSelectedEquipment

        private ICommand _cmdSeeSelectedEquipment;

        public ICommand CmdSeeSelectedEquipment
        {
            get
            {
                if (_cmdSeeSelectedEquipment == null)
                    _cmdSeeSelectedEquipment = new RelayCommand(ExSeeSelectedEquipment, CanExSeeSelectedEquipment, true);
                return _cmdSeeSelectedEquipment;
            }
        }

        private void ExSeeSelectedEquipment()
        {
            _dateTimes[2] = DateTime.Now;
            //UpdatePartolEventTaskInstanceInfo();
            this.OnEventScheduleChanged();


        }

        //private List<string> _lstSelected = new List<string>(); 
        //private void  GetSelectEquipment()
        //{
        //    _lstSelected.Clear();
        //    foreach (var t in ChildTreeItems )
        //    {
        //        if(t.IsChecked )
        //    }

        //}

        //private bool IsAllChildSelected(ListTreeNodeBase node)
        //{
        //    if (!node.IsChecked) return false;
        //    if (node.TreeNodeType == ListTreeNodeBase.NodeType.IsTml) return false;
        //    if (node.ChildTreeItems.Count < 1) return false;

        //    foreach (var t in node.ChildTreeItems)
        //    {
        //        if (!t.IsChecked) return false;
        //        if (t.TreeNodeType != ListTreeNodeBase.NodeType.IsTml)
        //        {
        //            if (!this.IsAllChildSelected(t))
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        //void CheckChildNode(ListTreeNodeBase node)
        //{

        //}

        private bool CanExSeeSelectedEquipment()
        {
            return DateTime.Now.Ticks - _dateTimes[2].Ticks > 30000000;
        }

        #endregion


        public void NavOnLoad(params object[] parsObjects)
        {
            Visi = Visibility.Visible;
            if (parsObjects.Length > 0)
            {
                var vInstance = parsObjects[0] as Wlst.client.EventSchduleTaskInstanceInfo;

               // IIEventSchduleTaskInstance vInstance = parsObjects[0] as IIEventSchduleTaskInstance;
                if (vInstance != null)
                {
                    int insId = vInstance.EventSchduleInstanceId;
                    this.RequsetPartolEventTaskInstanceInfo(insId);
                }
                else
                {
                    this.EventSchduleClassId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId ;
                    this.EventSchduleInstanceDescription = "No Description";
                    this.EventSchduleInstanceId = -1;
                    this.EventSchduleInstanceName = "Not Set";
                    this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId ;
                    this.EventSchduleInstanceDetail = "No Detail";
                    EventSchedule = new Wlst.client.EventSchedule();
                    this.AlreadyExcutedTimes = 0;
                    //todo
                }
            }
            else
            {
                this.EventSchduleClassId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId ;
                this.EventSchduleInstanceDescription = "No Description";
                this.EventSchduleInstanceId = -1;
                this.EventSchduleInstanceName = "Not Set";
                this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId ;
                this.EventSchduleInstanceDetail = "No Detail";
                EventSchedule = new Wlst.client.EventSchedule();
                this.AlreadyExcutedTimes = 0;
                //todo
            }
            // this.LoadNode();
            //this.IsShowLuxImageOnJpegMap = Setting.IsShowLuxImageOnJpegMap;
        }

        private void UpdateEventInstanceInfo(EventTaskInstanceInfo info)
        {
            this.UpdateEventInstanceViewModel(info);
            //this.EventSchedule = info.EventSchedule;
        }
    };

    /// <summary>
    /// Event
    /// </summary>
    public partial class WjEventScheduleViewVm
    {
       
        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_task_detail,//.ClientPart.wlst_EventInstancesPartol_server_ans_clinet_request_PartolLduLinesEventTaskInstance  ,
                LduEventTaskInstanceInfoRequsetOrReply,
                typeof(WjEventScheduleViewVm), this);
        }

        public void LduEventTaskInstanceInfoRequsetOrReply(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            var tmlInfoExchangeforServer = infos.WstSvrAnsCntRequestOrUpdateTaskDetailInfo ;
            //var tmlInfoExchangeforServer = args.GetParams()[1] as MruEventTaskInstanceInfoforExchange;
            if (tmlInfoExchangeforServer == null) return;
            this.UpdateEventInstanceInfo(tmlInfoExchangeforServer);

        }


    }

    /// <summary>
    /// 数据驱动 服务器请求数据
    /// </summary>
    public partial class WjEventScheduleViewVm
    {
        private void RequsetPartolEventTaskInstanceInfo(int id)
        {
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //MruTaskInstanceInfoRequest info = new MruTaskInstanceInfoRequest();
            //info.InstanceId = id;
            //SndOrderServer.OrderSnd(Wj1050Module.Services.EventIdAssign.MruEventTaskInstanceInfoRequsetOrReply,
            //                        null, info, gid);

            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_task_detail;//.ServerPart.wlst_EventInstancesPartol_clinet_request_PartolLduLinesEventTaskInstance ;
            info.WstCntRequestTaskDetailInfo .InstanceId = id;
            SndOrderServer.OrderSnd(info);

          //  LogInfo.Log("客户端请求线路检测巡测命令已经发送");
        }

        private void UpdatePartolEventTaskInstanceInfo()
        {
            //MruEventTaskInstanceInfoforExchange info = new MruEventTaskInstanceInfoforExchange()
            //                                               {
            //                                                   Info = this.GetMruEventTaskInstanceInfo()
            //                                               };
            //int gid = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;

            //SndOrderServer.OrderSnd(Wj1050Module.Services.EventIdAssign.MruEventTaskInstanceInfoUpdate,
            //                        null, info, gid);

            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_update_task_detail ;//.ServerPart.wlst_EventInstancesPartol_clinet_update_PartolLduLinesEventTaskInstance  ;
            info.WstCntUpdateTaskInfo = this.GetMruEventTaskInstanceInfo();
            SndOrderServer.OrderSnd(info);

          //  LogInfo.Log("用户更新线路检测巡测任务信息已经发送");
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(info.WstCntUpdateTaskInfo.InstanceInfo .EventSchduleInstanceId,
                                                                           info.WstCntUpdateTaskInfo.InstanceInfo .EventSchduleInstanceName,
                                                                           OperatrType.UserOperator,
                                                                           "更新或增加线路检测巡测任务信息");
        }

    }
}