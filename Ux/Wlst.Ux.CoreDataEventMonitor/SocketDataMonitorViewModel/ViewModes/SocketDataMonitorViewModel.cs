using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Commands;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.Services;

namespace Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.ViewModes
{

    [Export(typeof(IISocketDataMonitorViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SocketDataMonitorViewModel : ObservableObject, IISocketDataMonitorViewModel
    {
        public SocketDataMonitorViewModel()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {
            //事件记录、标记、日志、提示等
            try
            {
                if (args.EventType == EventTypeAssign.SocketServerRcvDataOri)
                {
                    this.AddItems(new ItemInfo() {SocketDataType = "Rcv Ori", Data = args.GetParams()[0].ToString()});
                }
                else if (args.EventType == EventTypeAssign.SocketServerSndDataOri)
                {
                    this.AddItems(new ItemInfo() {SocketDataType = "Snd Ori", Data = args.GetParams()[0].ToString()});
                }
                else if (args.EventType == EventTypeAssign.SocketServerSndDataProtocol )
                {
                    this.AddItems(new ItemInfo()
                                      {
                                          SocketDataType = "Snd Pcl",
                                          Id = args.GetParams()[0].ToString(),
                                          Guid = args.GetParams()[1].ToString(),
                                          Cmd = args.GetParams()[2].ToString(),
                                          OtherArug = args.GetParams()[3].ToString(),
                                          Data = args.GetParams()[4].ToString()
                                      });
                }
                else if (args.EventType == EventTypeAssign.SocketServerRcvDataProtocol)
                {
                    this.AddItems(new ItemInfo()
                                      {
                                          SocketDataType = "Rcv Pcl",
                                          Id = args.GetParams()[0].ToString(),
                                          Guid = args.GetParams()[1].ToString(),
                                          Cmd = args.GetParams()[2].ToString(),
                                          OtherArug = args.GetParams()[3].ToString(),
                                          Data = args.GetParams()[4].ToString()
                                      });
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                //contents += "fail";
            }
            try
            {
                // this.AddItems(new ItemInfo() { EventType = eventtype, EventId = eventid, EventPars = eventargs });
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core_Monitor SocketDataMonitorViewModel FundEventHandler Occer an error :" +
                                       ex.ToString() + ";data is:" + args.ToString());
            }
        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            // TerminalInformation 
            if (_bolMonitor)
            {
                if (args.EventType == EventTypeAssign.SocketServerRcvDataOri)
                {
                    return true;
                }
                else if (args.EventType == EventTypeAssign.SocketServerSndDataOri)
                {
                    return true;
                }
                else if (args.EventType == EventTypeAssign.SocketServerRcvDataProtocol)
                {
                    return true;
                }
                else if (args.EventType == EventTypeAssign.SocketServerSndDataProtocol  )
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        #endregion



        #region IITab

        /// <summary>
        /// 当显示在主界面的tab页面时 显示的title
        /// </summary>
        public string Title
        {
            get { return "底层Socket数据信息"; }
        }

        /// <summary>
        /// 当显示在主界面tab时是否允许用户关闭  地图不运行关闭
        /// </summary>
        public bool CanClose
        {
            get { return true; }
        }

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

        #endregion

        private void AddItems(ItemInfo item)
        {
            try
            {
                Items.Add(item);
                if (Items.Count > 500) Items.Clear();
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        private ObservableCollection<ItemInfo> _iitems;

        public ObservableCollection<ItemInfo> Items
        {
            get
            {
                if (_iitems == null) _iitems = new ObservableCollection<ItemInfo>();
                return _iitems;
            }
        }


        private bool _bolMonitor;
        private ICommand _cmdMonitor;

        public ICommand CmdMonitor
        {
            get
            {
                if (_cmdMonitor == null) _cmdMonitor = new RelayCommand(ExMoni );
                return _cmdMonitor;
            }
        }

        private void ExMoni()
        {
            _bolMonitor = !_bolMonitor;
            Items.Clear();
            EventTypeAssign.MonitorSocketData = _bolMonitor;
            this.RaisePropertyChanged(() => this.CmdName);
        }



        public string CmdName
        {
            get
            {
                if (!_bolMonitor)
                    return "Moni";
                else
                {
                    return "No Moni";
                }
            }
        }
    }
}
