using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.Setting.EventTaskViewModel.Services;
using Wlst.Ux.Setting.EventTaskViewModel.View;
using Wlst.client;

namespace Wlst.Ux.Setting.EventTaskViewModel.ViewModel
{
    [Export(typeof (IIEventTaskViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EventTaskViewModel :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
        Services.IIEventTaskViewModel
    {
        public EventTaskViewModel()
        {
           InitAction();
            InitEvent();
            OneArea = Visibility.Collapsed;
            Msg = "";
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EventTaskAllNeedUpdate)
            {
                if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
                {

                    Msg = DateTime.Now + " 更新成功!";
                }
                else
                {
                    Msg = DateTime.Now + " 收到更新数据!";
                }
            
            }
        }

        public void InitEvent()
        {           
            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EventTaskAllNeedUpdate);
        }

        #region Tab
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
            get { return "计划任务"; }
        }

        #endregion

        public void NavOnLoad(params object[] parsObjects)
        {           
            this.ItemsArea.Clear();
            if (UserInfo.UserLoginInfo.D == true)
            {               
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = f.Key.ToString("d2") + "-" + f.Value.AreaName,
                        Value = f.Key
                    });
                }
            }
            else
            {
                List<int> areaLst = new List<int>();
                areaLst.AddRange(UserInfo.UserLoginInfo.AreaX);                              
                foreach (var f in areaLst)
                {
                    var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(f);
                    this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = areaInfo.AreaId.ToString("d2") + "-" + areaInfo.AreaName,
                        Value = areaInfo.AreaId
                    });
                }

            }

            if (ItemsArea.Count > 0) CurrentSelectArea = ItemsArea[0];
            OneArea  = ItemsArea.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            RequestTaskInfo();
        }

       public void LoadOneArea()
       {
           var tmp = new List<EventTaskItemViewModel>();
           TaskItems.Clear();
           if (AllTaskItems==null) return;
           if (CurrentSelectArea == null) return;
           foreach(var t in AllTaskItems)
           {
               if(t.AreaId == CurrentSelectArea.Value )
               {
                   
                   TaskItems.Add(t);
               }
           }
       }

       public void FirstTimeLoadOneArea()
       {
           TaskItems.Clear();
           if (AllTaskItems == null) return;
           if (CurrentSelectArea == null) return;
           foreach (var t in AllTaskItems)
           {
               if (t.AreaId == CurrentSelectArea.Value)
               {
                   TaskItems.Add(t);
               }
           }
       }

        #region Attribute

       private DateTime dtSnd;
       private DateTime _dtSave;

       private string _msg;
       /// <summary>
       /// 提示
       /// </summary>
       public string Msg
       {
           get { return _msg; }
           set
           {
               if (value != _msg)
               {
                   _msg = value;
                   this.RaisePropertyChanged(() => this.Msg);
               }
           }
       }

       private Visibility _onearea;
       /// <summary>
       /// 区域选择框是否隐藏
       /// </summary>
       public Visibility OneArea
       {
           get { return _onearea; }
           set
           {
               if (value != _onearea)
               {
                   _onearea = value;
                   this.RaisePropertyChanged(() => this.OneArea);
               }
           }
       }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _fItemsArea;
        /// <summary>
        /// 区域选择combobox
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ItemsArea
        {
            get
            {
                if (_fItemsArea == null)
                {
                    _fItemsArea = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _fItemsArea;
            }
            set
            {
                if (value == _fItemsArea) return;
                _fItemsArea = value;
                this.RaisePropertyChanged(() => ItemsArea);
            }
        }
        
        private Wlst.Cr.CoreOne.Models.NameValueInt _cur;
        /// <summary>
        /// 选中区域
        /// </summary>
        public Wlst.Cr.CoreOne.Models.NameValueInt CurrentSelectArea
        {
            get { return _cur; }
            set
            {
                if (value == _cur) return;
                _cur = value;
                this.RaisePropertyChanged(() => this.CurrentSelectArea);
               LoadOneArea();

            }
        }
        #endregion


        #region 事件

        //将巡测时间转换成int
        public static int TransferToInt(string minutes)
        {
            var splitminutes = minutes.Split(':');
            int hour = Convert.ToInt32(splitminutes[0]);
            int min = Convert.ToInt32(splitminutes[1]);
            return hour * 60 + min;
        }

        public List<AttachEquPartolSchdule.AttachEquPartolSchduleItem> GetTaskInfo()
        {
            var list = new List<AttachEquPartolSchdule.AttachEquPartolSchduleItem>();
            foreach(var t in TaskItems )
            {
                //t.StartMinutes = TransferToInt(t.TaskStTime);
                var tmp = new AttachEquPartolSchdule.AttachEquPartolSchduleItem()
                              {
                                  AreaId = t.AreaId,
                                  EquType = t.EquType,
                                  StartMinutes = t.StartMinutes,
                                  MinutesIntervals = t.Interval,
                                  MinutesIntervalsRePartol = t.MinutesIntervalsRePartol,
                                  TimesRePartol = t.TimesRepartol
                              };
                list.Add(tmp);
            }
            return list;
        }

        private bool IsDataValidate(ObservableCollection<EventTaskItemViewModel> allTaskItems)
        {
            foreach (var t in allTaskItems)
            {
                if (t.AreaId == CurrentSelectArea.Value)
                {
                    if (t.StartMinutes >= 1440)
                    {
                        UMessageBox.Show("设置错误", "存在错误的时间设置，请重新设置", UMessageBoxButton.Yes);
                        t.StartMinutes = 30;
                        
                        return false;
                    }
                    if (t.EquType == 1 || t.EquType == 2 || t.EquType == 5 || t.EquType == 7)
                    {
                        if (t.Interval < 10 || t.Interval > 180)
                        {
                            UMessageBox.Show("设置错误", "终端、单灯、节电、线路检测巡测间隔须在10分钟到3小时之间", UMessageBoxButton.Yes);
                            t.Interval = 10;
                            return false;
                        }
                    }
                   if(t.MinutesIntervalsRePartol *t.TimesRepartol > t.Interval )
                   {
                       UMessageBox.Show("设置错误", "补测次数与补测间隔的乘积不能大于执行间隔", UMessageBoxButton.Yes);                      
                       return false;
                   }
                }
            }
            return true;
        }

        #endregion

        private ObservableCollection<EventTaskItemViewModel> _taskItems;
        /// <summary>
        /// 计划任务列表
        /// </summary>
        public ObservableCollection<EventTaskItemViewModel> TaskItems
        {
            get
            {
                if (_taskItems == null)
                    _taskItems = new ObservableCollection<EventTaskItemViewModel>();
                return _taskItems;
            }
        }

        private EventTaskItemViewModel _currentTaskItems;
        /// <summary>
        /// 当前选中任务列表
        /// </summary>
        public EventTaskItemViewModel CurrentTaskItems
        {
            get
            {
                if (_currentTaskItems == null)
                    _currentTaskItems = new EventTaskItemViewModel(new AttachEquPartolSchdule.AttachEquPartolSchduleItem());
                return _currentTaskItems;
            }           
            set
            {
                if (value != _currentTaskItems)
                {
                    _currentTaskItems = value;
                    this.RaisePropertyChanged(() => this.CurrentTaskItems);
                }
            }
        }

        private DateTime[] _dateTimes = new DateTime[3];
        private ICommand _CmdSave;
        /// <summary>
        /// 保存按钮
        /// </summary>
        public ICommand CmdSave
        {
            get
            {
                if (_CmdSave == null)
                {
                    _CmdSave = new RelayCommand(ExSave, CanSave, false);
                }
                return _CmdSave;
            }
        }

        private void ExSave()
        {
            _dtSave = DateTime.Now;
            if (!IsDataValidate(AllTaskItems)) return;
            UpdateTaskInfo(GetTaskInfo());
            Msg = DateTime.Now + " 已经提交更新信息到服务器，请等待...";
            var arg = new PublishEventArgs()
            {
                EventId = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EventTaskAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
            dtSnd = DateTime.Now;
            
        }

        private bool CanSave()
        {
            return DateTime.Now.Ticks - _dtSave.Ticks > 30000000;
        }
       

    }

    /// <summary>
    /// 数据驱动 事件
    /// </summary>
    public partial class EventTaskViewModel
    {

        public ObservableCollection<EventTaskItemViewModel> AllTaskItems =new ObservableCollection<EventTaskItemViewModel>();
             

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                 Wlst.Sr.ProtocolPhone.LxSys.wst_sys_partol_set,
                 LoadItems,
                 typeof(EventTaskViewModel), this,true);
        }

        

        private void RequestTaskInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_partol_set;
            
            info.WstSysPartolSet.Op = 1;
            info.WstSysPartolSet.AreaId = CurrentSelectArea.Value;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public void LoadItems(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstSysPartolSet;
            if(info == null)return;
            TaskItems.Clear();
            AllTaskItems.Clear();
            var order = from t in info.Items orderby t.EquType ascending select t;
            foreach (var t in order)
            {
                AllTaskItems.Add(new EventTaskItemViewModel(t));
            }
            foreach(var t in AllTaskItems )
            {
                if (t.TaskName == "定时抄表" || t.TaskName == "光控巡测") t.Is2090 = false ;
                else t.Is2090 = true;
                if (t.TaskName == "光控巡测") t.IsLux = false;
                else t.IsLux = true;
            }
            FirstTimeLoadOneArea();
            //var arg = new PublishEventArgs()
            //{
            //    EventId = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EventTaskAllNeedUpdate,
            //    EventType = PublishEventType.Core
            //};
            //EventPublish.PublishEvent(arg);
        }

        private void UpdateTaskInfo(List<AttachEquPartolSchdule.AttachEquPartolSchduleItem> taskinfo)
        {
  

            var info = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_partol_set;

            foreach (var t in taskinfo)
            {
                info.WstSysPartolSet.Items.Add(new AttachEquPartolSchdule.AttachEquPartolSchduleItem()
                                                   {
                                                       AreaId = CurrentSelectArea.Value,
                                                       EquType = t.EquType,
                                                       StartMinutes = t.StartMinutes ,
                                                       MinutesIntervals = t.MinutesIntervals,
                                                       MinutesIntervalsRePartol = t.MinutesIntervalsRePartol,
                                                       TimesRePartol = t.TimesRePartol


                                                   });


            }
            info.WstSysPartolSet.Op = 2;
            info.WstSysPartolSet.AreaId = CurrentSelectArea.Value;

            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "当前区域", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新计划任务");
           
            RequestTaskInfo();
        }



    }
}