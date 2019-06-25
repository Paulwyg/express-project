using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.StateBarModule.NewSvrMsgView.Services;

namespace Wlst.Ux.StateBarModule.NewSvrMsgView.ViewModel
{
    //[Export(typeof (IIOperatorOnTimeRecords))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public class NewSvrMsgViewModel : Wlst.Cr.Core.CoreServices .ObservableObject , IIOperatorOnTimeRecords
    {
        public NewSvrMsgViewModel()
        {
            InitAction();
            IsChecked = false;

           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

           // Wlst .Cr .Core.ModuleServices .DelayEvent .RegisterDelayEvent(OnLoadCpt ,2,DelayEventHappen.EventOne);
        }
        
        //void OnLoadCpt()
        //{
        //    isload = true;
        //}


        private bool _sIsChecked;

        public bool IsChecked
        {
            get { return _sIsChecked; }
            set
            {
                if (value != _sIsChecked)
                {
                    _sIsChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {
            try
            {
                //Async.Run(new Action<object>(ExExecuteEvent), args);

                AddNewRecordItem(args.EventId, args.EventType, OperatrType.SystemInfo,
                                 args.GetParams().Count > 0 ? args.GetParams()[0].ToString() : "None");

            }
            catch (Exception ex)
            {
            }
        }

   //     private bool isload = false;
        private bool _isfilter;
        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool FundOrderFilter(PublishEventArgs args)
        {
            return _isfilter;
        
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone .LxSys  .wlst_sys_svr_to_cnt_info ,//.ClientPart.wlst_Infrastructure_server_to_clinet_msginfo,
                                          MsgAction, typeof(NewSvrMsgViewModel), this, true);
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.ActionAddShowInfo += AddNewRecordItem;

           
            
        }

        private void MsgAction(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            if (infos.WstSysSvrToCntInfo == null) return;
            AddNewRecordItem(infos.WstSysSvrToCntInfo.RtuId, infos.WstSysSvrToCntInfo.RtuName, OperatrType.SystemInfo,
                             infos.WstSysSvrToCntInfo.Msg);
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
            get { return "最新信息"; }
        }

        #endregion

        private ObservableCollection<OperatorRecordItem> _records;

        /// <summary>
        /// 操作码+操作终端+操作回路 如222222+1+3  操作码22222 1终端3回路
        /// 操作码+操作终端+操作集合 如 22222+1+135  操作码22222 1终端1、3、5回路
        /// </summary>
        public ObservableCollection<OperatorRecordItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OperatorRecordItem>();
                return _records;
            }
        }

        //private OperatorRecordItem curr;
        //public OperatorRecordItem CurrentSelectItem
        //{
        //    get { return curr; }
        //    set
        //    {
        //        if (value == curr) return;
        //        curr = value;
        //        this.RaisePropertyChanged(() => this.CurrentSelectItem);
        //    }
        //}

        #region CmdDelete

        private DateTime _dtDelete;

        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExCmdDelete, CanExCmdDelete, true); }
        }

        private void ExCmdDelete()
        {
            _dtDelete = DateTime.Now;
            this.Records.Clear();

        }

        private bool CanExCmdDelete()
        {
            return Records.Count > 0 && DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
        }

        #endregion


        /// <summary>
        /// 新增加新的用户操作信息
        /// </summary>
        /// <param name="rtuId">操作的终端地址</param>
        /// <param name="rtuName">终端 </param>
        /// <param name="operatr">用户操作还是服务器应答 </param>
        /// <param name="operatorContent">执行情况 如 完成或 等待 </param>
        public void AddNewRecordItem(int rtuId, string rtuName, OperatrType operatr, string operatorContent)
        {
           // if (isload == false) return;
            //return;
            if (IsChecked) return;
            var ins = new OperatorRecordItem()
                          {
                              RtuName = rtuName,
                              RtuId = rtuId,
                              Operatr = operatr,
                              OperatorContent = operatorContent,
                              OpTime = DateTime.Now
                          };
            Records.Insert(0, ins);
            CurrentItem = ins;

            if (Records.Count > 100)
            {
                Records.Clear();
            }
        }

        private OperatorRecordItem _cr;

        public OperatorRecordItem CurrentItem
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


        private int count = 0;
        private DateTime dtLast=DateTime .Now ;
        public void CurrentSelectItemDoubleClicked()
        {
            try
            {
                if (DateTime.Now.Ticks - dtLast.Ticks < 50000000) count++;
                else count = 0;
                if (count > 3) _isfilter = true;
                else _isfilter = false;
                dtLast = DateTime.Now;

                if (CurrentItem == null) return;
                if (CurrentItem.RtuId < 100) return;
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
            catch (Exception ex){}
        }

    }
}
